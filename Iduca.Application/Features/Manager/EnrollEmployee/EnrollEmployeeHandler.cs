using Iduca.Application.Common.Exceptions;
using Iduca.Application.Common.Services;
using Iduca.Application.Common.Session;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Iduca.Application.Features.Manager.EnrollEmployee;

public class EnrollEmployeeHandler(
    IUserRepository userRepository,
    ICourseRepository courseRepository,
    IUserCourseRepository userCourseRepository,
    IHierarchyService hierarchyService,
    IRequestSession requestSession,
    IUnitOfWork unitOfWork,
    ILogger<EnrollEmployeeHandler> logger)
    : IRequestHandler<EnrollEmployeeRequest, EnrollEmployeeResponse>
{
    public async Task<EnrollEmployeeResponse> Handle(EnrollEmployeeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var sessionData = requestSession.GetSessionOrThrow();
            var currentUserId = sessionData.UserId;
            
            // Verificar se o manager tem permissão para inscrever este funcionário
            if (!await hierarchyService.CanUserAccessDataAsync(currentUserId, request.EmployeeId, cancellationToken))
            {
                throw new BadRequestException("Você não tem permissão para inscrever este funcionário em cursos.");
            }

            // Verificar se o funcionário existe
            var employee = await userRepository.Get(request.EmployeeId, cancellationToken);
            if (employee == null)
            {
                throw new NotFoundException("Funcionário não encontrado.");
            }

            // Verificar se o curso existe
            var course = await courseRepository.Get(request.CourseId, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException("Curso não encontrado.");
            }

            // Verificar se o funcionário já está inscrito no curso
            var existingEnrollment = await userCourseRepository.GetUserCourseByIds(
                request.EmployeeId, 
                request.CourseId, 
                cancellationToken);
                
            if (existingEnrollment != null)
            {
                throw new DuplicityException("O funcionário já está inscrito neste curso.");
            }

            // Criar nova inscrição
            var userCourse = new UserCourse
            {
                Id = Guid.NewGuid(),
                UserId = request.EmployeeId,
                User = employee,
                CourseId = request.CourseId,
                Course = course,
                CreatedAt = DateTime.UtcNow,
                EndDate = null,
                Rating = null,
                Certificate = null
            };

            // Salvar a inscrição
            userCourseRepository.Create(userCourse);
            await unitOfWork.Save(cancellationToken);

            logger.LogInformation(
                "Manager {ManagerId} enrolled employee {EmployeeId} in course {CourseId}",
                currentUserId,
                request.EmployeeId,
                request.CourseId);

            return new EnrollEmployeeResponse
            {
                Success = true,
                Message = $"Funcionário {employee.Name} foi inscrito com sucesso no curso {course.Name}.",
                UserCourseId = userCourse.Id
            };
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, 
                "Error enrolling employee {EmployeeId} in course {CourseId} by manager {ManagerId}",
                request.EmployeeId,
                request.CourseId,
                requestSession.GetSessionOrThrow().UserId);
                
            throw new BadRequestException("Erro interno ao inscrever funcionário no curso.");
        }
    }
}
