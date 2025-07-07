using Iduca.Api.Attributes;
using Iduca.Application.Repository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository.CategoryRepository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route("api/profile")]
[CustomAuthorize] // Requer autenticação para todas as rotas
public class ProfileController(IMediator mediator) : BaseController
{
    private readonly IMediator mediator = mediator;

    [HttpGet]
    public async Task<ActionResult> GetProfile(CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();
        
        var userRepository = HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var userCourseRepository = HttpContext.RequestServices.GetRequiredService<IUserCourseRepository>();
        
        var user = await userRepository.Get(currentUserId, cancellationToken);
        if (user == null)
        {
            return NotFound(new { message = "Usuário não encontrado" });
        }
        
        var userCourses = await userCourseRepository.GetAllByUserId(currentUserId, cancellationToken);
        var completedCourses = userCourses.Where(uc => uc.EndDate.HasValue).ToList();
        
        // Calcular média dos ratings (avaliações dos cursos)
        var averageRating = completedCourses.Any() && completedCourses.Any(c => c.Rating.HasValue)
            ? Math.Round(completedCourses.Where(c => c.Rating.HasValue).Average(c => c.Rating!.Value), 1)
            : 0.0;
        
        var response = new
        {
            photoUser = string.IsNullOrEmpty(user.Image) ? "https://via.placeholder.com/150" : user.Image,
            name = user.Name,
            email = user.Email,
            interests = user.Interests?.Select(i => i.Name).ToArray() ?? new string[0],
            completedCourses = completedCourses.Count,
            completedCoursesList = completedCourses.Select(uc => new
            {
                id = uc.Course.Id,
                title = uc.Course.Name,
                image = string.IsNullOrEmpty(uc.Course.Image) ? "https://via.placeholder.com/300x200" : uc.Course.Image,
                certificateAvailable = !string.IsNullOrEmpty(uc.Certificate),
                completedAt = uc.EndDate,
                rating = uc.Rating
            }).ToList()
        };

        return Ok(response);
    }

    [HttpGet("certificate/{id}/image")]
    public async Task<ActionResult> GetCertificateImage(Guid id, CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();
        var userCourseRepository = HttpContext.RequestServices.GetRequiredService<IUserCourseRepository>();
        
        var userCourse = await userCourseRepository.GetUserCourseByIds(currentUserId, id, cancellationToken);
        
        if (userCourse == null)
        {
            return NotFound(new { message = "Usuário não está inscrito neste curso" });
        }
        
        if (!userCourse.EndDate.HasValue)
        {
            return BadRequest(new { message = "Curso ainda não foi concluído" });
        }
        
        if (string.IsNullOrEmpty(userCourse.Certificate))
        {
            return NotFound(new { message = "Certificado não disponível para este curso" });
        }
        
        // Por enquanto retornando URL do certificado
        // TODO: Implementar geração/busca real da imagem do certificado
        return Ok(new { 
            certificateImageUrl = userCourse.Certificate,
            courseName = userCourse.Course.Name,
            userName = userCourse.User.Name,
            completedAt = userCourse.EndDate
        });
    }

    [HttpGet("certificate/{id}/pdf")]
    public async Task<ActionResult> GetCertificatePdf(Guid id, CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();
        var userCourseRepository = HttpContext.RequestServices.GetRequiredService<IUserCourseRepository>();
        
        var userCourse = await userCourseRepository.GetUserCourseByIds(currentUserId, id, cancellationToken);
        
        if (userCourse == null)
        {
            return NotFound(new { message = "Usuário não está inscrito neste curso" });
        }
        
        if (!userCourse.EndDate.HasValue)
        {
            return BadRequest(new { message = "Curso ainda não foi concluído" });
        }
        
        if (string.IsNullOrEmpty(userCourse.Certificate))
        {
            return NotFound(new { message = "Certificado não disponível para este curso" });
        }
        
        // TODO: Implementar geração real do PDF do certificado
        // Por enquanto retornando informações para download
        return Ok(new { 
            downloadUrl = $"/downloads/certificado_{userCourse.Course.Name.Replace(" ", "_")}_{currentUserId}.pdf",
            fileName = $"Certificado_{userCourse.Course.Name.Replace(" ", "_")}.pdf",
            courseName = userCourse.Course.Name,
            userName = userCourse.User.Name,
            completedAt = userCourse.EndDate
        });
    }


    [HttpGet("interests")]
    [AllowAnonymous]
    public async Task<ActionResult> GetInterests(CancellationToken cancellationToken = default)
    {
        var categoryRepository = HttpContext.RequestServices.GetRequiredService<ICategoryRepository>();
        var categories = await categoryRepository.GetAll(cancellationToken);
        
        var interests = categories.Select(c => new 
        { 
            id = c.Id, 
            name = c.Name 
        }).ToList();

        return Ok(interests);
    }

    [HttpPut("profile")]
    public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Interests != null && request.Interests.Length > 5)
        {
            return BadRequest(new { message = "Máximo de 5 interesses permitidos" });
        }
        
        var currentUserId = GetCurrentUserId();
        var userRepository = HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var categoryRepository = HttpContext.RequestServices.GetRequiredService<ICategoryRepository>();
        var unitOfWork = HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
        
        var user = await userRepository.Get(currentUserId, cancellationToken);
        if (user == null)
        {
            return NotFound(new { message = "Usuário não encontrado" });
        }
        
        if (!string.IsNullOrEmpty(request.PhotoUser))
        {
            user.Image = request.PhotoUser;
        }
        
        if (request.Interests != null)
        {
            var allCategories = await categoryRepository.GetAll(cancellationToken);
            var validCategoryIds = allCategories.Select(c => c.Id).ToList();
            
            var invalidCategories = request.Interests.Where(id => !validCategoryIds.Contains(id)).ToList();
            if (invalidCategories.Any())
            {
                return BadRequest(new { 
                    message = "Algumas categorias são inválidas", 
                    invalidIds = invalidCategories 
                });
            }
            
            user.Interests.Clear();
            foreach (var categoryId in request.Interests)
            {
                var category = allCategories.First(c => c.Id == categoryId);
                user.Interests.Add(category);
            }
        }
        
        userRepository.Update(user);
        await unitOfWork.Save(cancellationToken);
        
        return Ok(new { 
            message = "Perfil atualizado com sucesso",
            photoUser = user.Image,
            interests = user.Interests.Select(i => new { id = i.Id, name = i.Name }).ToList()
        });
    }
}

public class UpdateProfileRequest
{
    public string? PhotoUser { get; set; }
    public Guid[]? Interests { get; set; }
}
