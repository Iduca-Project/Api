using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Courses.Enroll;

public class EnrollCourseHandler(
    ICourseRepository courseRepository,
    IUserRepository userRepository,
    IUserCourseRepository userCourseRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<EnrollCourseRequest, EnrollCourseResponse>
{
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly IUserCourseRepository userCourseRepository = userCourseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<EnrollCourseResponse> Handle(EnrollCourseRequest request, CancellationToken cancellationToken)
    {
        // Verificar se o curso existe
        var course = await courseRepository.GetCourseByEqualName(request.CourseName, cancellationToken)
            ?? throw new NotFoundException("Curso não encontrado.");

        // Verificar se o usuário existe
        var user = await userRepository.GetUserByIdentity(request.Identity, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        // Verificar se o usuário já está matriculado
        var existingEnrollment = await userCourseRepository.GetUserCourseByIds(user.Id, course.Id, cancellationToken);
        if (existingEnrollment is not null)
            throw new DuplicityException("Usuário já está matriculado neste curso.");

        // Criar nova matrícula
        var userCourse = new UserCourse
        {
            UserId = user.Id,
            CourseId = course.Id,
            User = user,
            Course = course,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        userCourseRepository.Create(userCourse);
        await unitOfWork.Save(cancellationToken);

        return new EnrollCourseResponse(
            user.Id,
            course.Id,
            userCourse.CreatedAt,
            "Matrícula realizada com sucesso"
        );
    }
}
