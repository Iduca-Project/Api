using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Domain.Models;
using Iduca.Domain.Common.Messages;
using Iduca.Application.Common.Exceptions;
using MediatR;
using Iduca.Application.Features.Companies.Create;
using Iduca.Application.Repository.CategoryRepository;
using Iduca.Application.Repository.ModuleRepository;
using Iduca.Application.Repository.ExamRepository;
using Iduca.Application.Repository.LessonRepository;
using Iduca.Application.Repository.ExerciseRepository;
using Iduca.Application.Repository.QuestionRepository;
using Iduca.Application.Repository.AlternativeRepository;
using Iduca.Application.Repository.ContentRepository;

namespace Iduca.Application.Features.Courses.Create;

public class CreateCourseHandler (
    ICourseRepository courseRepository,
    ICategoryRepository categoryRepository,
    IModuleRepository moduleRepository,
    ILessonRepository lessonRepository,
    IQuestionRepository questionRepository,
    IAlternativeRepository alternativeRepository,
    IExerciseRepository exerciseRepository,
    IExamRepository examRepository,
    IContentRepository contentRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<CreateCourseRequest, CreateCourseResponse>
{
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly ILessonRepository lessonRepository = lessonRepository;
    private readonly IExerciseRepository exerciseRepository = exerciseRepository;
    private readonly IQuestionRepository questionRepository = questionRepository;
    private readonly IAlternativeRepository alternativeRepository = alternativeRepository;
    private readonly IModuleRepository moduleRepository = moduleRepository;
    private readonly IContentRepository contentRepository = contentRepository;
    private readonly IExamRepository examRepository = examRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<CreateCourseResponse> Handle(CreateCourseRequest request, CancellationToken cancellationToken)
    {
        var newCourse = new Course()
        {
            Name = request.Title,
            Description = request.Description,
            Difficulty = (int)request.Difficulty,
            Image = request.Image,
            TotalHours = request.Duration,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var findCourse = await courseRepository.GetCourseByEqualName(request.Title, cancellationToken);
        if (findCourse is not null)
            throw new DuplicityException(ExceptionMessage.DuplicityModel.Default);
        var categoriesFromDb = new List<Category>();
        foreach (var id in request.Categories)
        {
            var findCategory = await categoryRepository.Get(id, cancellationToken)
                ?? throw new NotFoundException(ExceptionMessage.NotFound.Default);
            categoriesFromDb.Add(findCategory);
        }
        newCourse.Categories = categoriesFromDb;
        courseRepository.Create(newCourse);

        if (request.Exam is not null)
        {
            var newExam = new Exam()
            {
                Course = newCourse,
                CourseId = newCourse.Id,
                Description = request.Exam.Description,
                Title = request.Exam.Title,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Date = DateTime.UtcNow
            };
            examRepository.Create(newExam);
            
            foreach(var question in request.Exam.Questions)
            {
                var newQuestion = new Question
                {
                    Title = question.Description,
                    Exams = [newExam],
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                questionRepository.Create(newQuestion);
                
                foreach(var alternative in question.Alternatives)
                {
                    var newAlternative = new Alternative
                    {
                        Question = newQuestion,
                        QuestionId = newQuestion.Id,
                        Description = alternative.Description,
                        IsCorrect = alternative.IsCorrect,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    alternativeRepository.Create(newAlternative);
                }
            }
        }


        foreach (var module in request.Modules)
        {
            Module? findModule = await moduleRepository.GetModuleByEqualNameInCourse(module.Name, newCourse.Id, cancellationToken);
            if (findModule is not null)
                throw new DuplicityException(ExceptionMessage.DuplicityModel.ModuleNameDuplicity);

            var newModule = new Module
            {
                Name = module.Name,
                Description = module.Description,
                Index = 0,
                Course = newCourse,
                CourseId = newCourse.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            moduleRepository.Create(newModule);


            if (module.Lessons != null)
                foreach (var lesson in module.Lessons)
                {
                    var newLesson = new Lesson
                    {
                        Title = lesson.Name,
                        Description = lesson.Description,
                        Module = newModule,
                        ModuleId = newModule.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    lessonRepository.Create(newLesson);

                    foreach (var content in lesson.Contents)
                    {
                        var newContent = new Content
                        {
                            Lesson = newLesson,
                            LessonId = newLesson.Id,
                            Description = content.Description ?? "",
                            Title = content.Title,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            Image = content.Image,
                            Index = 0
                        };
                        contentRepository.Create(newContent);
                    }
                }

            if (module.Exercises != null)
                foreach (var exercise in module.Exercises)
                {
                    var newExercise = new Exercise
                    {
                        Title = exercise.Title,
                        Description = exercise.Description,
                        Module = newModule,
                        ModuleId = newModule.Id,
                        Date = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    exerciseRepository.Create(newExercise);

                    foreach (var question in exercise.Questions)
                    {
                        var newQuestion = new Question
                        {
                            Title = question.Description,
                            Exercises = [newExercise],
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                        };
                        questionRepository.Create(newQuestion);

                        foreach (var alternative in question.Alternatives)
                        {
                            var newAlternative = new Alternative
                            {
                                Question = newQuestion,
                                QuestionId = newQuestion.Id,
                                Description = alternative.Description,
                                IsCorrect = alternative.IsCorrect,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                            };
                            alternativeRepository.Create(newAlternative);
                        }
                    }
                }
        }








        await unitOfWork.Save(cancellationToken);

        return mapper.Map<CreateCourseResponse>(newCourse);
    }
}