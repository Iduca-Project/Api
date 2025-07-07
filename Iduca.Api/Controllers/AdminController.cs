using Iduca.Api.Attributes;
using Iduca.Application.Features.Companies.Create;
using Iduca.Application.Features.Companies.Update;
using Iduca.Application.Features.Categories.Create;
using Iduca.Application.Features.Categories.DeleteById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Iduca.Application.Features.Companies.GetAll;
using Iduca.Application.Features.Courses.Create;
using Iduca.Application.Features.Modules.Create;
namespace Iduca.Api.Controllers;

[ApiController]
[Route("api/admin")]
[AdminAuthorize] // TODO: Adicionar atributo específico para Admin quando disponível
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Empresas

    /// <summary>
    /// Lista todas as empresas cadastradas no sistema
    /// </summary>
    [HttpGet("companies")]
    public async Task<ActionResult<GetAllCompanyResponse>> GetAllCompany([FromQuery] GetAllCompanyRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Cadastra uma nova empresa
    /// </summary>
    [HttpPost("companies")]
    public async Task<ActionResult<CreateCompanyResponse>> CreateCompany([FromBody] CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Created($"/api/admin/companies", response);
    }

    /// <summary>
    /// Deleta uma empresa e funcionários vinculados
    /// </summary>
    [HttpDelete("companies/{companyId}")]
    public ActionResult DeleteCompany([FromRoute] int companyId, CancellationToken cancellationToken)
    {
        // TODO: Implementar DeleteCompanyRequest e Handler
        var mockResponse = new
        {
            message = "Company and related employees deleted successfully"
        };

        return Ok(mockResponse);
    }

    /// <summary>
    /// Atualiza uma empresa existente
    /// </summary>
    [HttpPut("companies/{companyId}")]
    public async Task<ActionResult<UpdateCompanyResponse>> UpdateCompany(
        [FromRoute] Guid companyId, 
        [FromBody] UpdateCompanyRequestProps props, 
        CancellationToken cancellationToken = default)
    {
        var request = new Application.Features.Companies.Update.UpdateCompanyRequest(companyId, props);
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }




    #endregion

    #region Modules
    [HttpPost("module")]
    public async Task<ActionResult<CreateModuleResponse>> Create(
        CreateModuleRequest request, CancellationToken cancellationToken
    )
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    #endregion



    #region Cursos

    /// <summary>
    /// Cria um curso novo com módulos e conteúdos
    /// </summary>
    [HttpPost("course")]
    public async Task<ActionResult> CreateCourseAsync([FromBody] CreateCourseRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Deleta um curso
    /// </summary>
    [HttpDelete("course/{idCourse}")]
    public ActionResult DeleteCourse([FromRoute] Guid idCourse, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar DeleteCourseAdminRequest e Handler
        var mockResponse = new
        {
            message = "Course deleted successfully"
        };

        return Ok(mockResponse);
    }

    #endregion

    #region Managers

    /// <summary>
    /// Cria um manager novo e vincula a uma empresa
    /// </summary>
    [HttpPost("managers")]
    public ActionResult CreateManager([FromBody] CreateManagerRequest request, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar CreateManagerRequest e Handler
        var mockResponse = new
        {
            message = "Manager created and linked to company successfully",
            managerId = 15
        };

        return Ok(mockResponse);
    }

    #endregion

    #region Categorias

    /// <summary>
    /// Criar nova categoria
    /// </summary>
    [HttpPost("categories")]
    public async Task<ActionResult<CreateCategoryResponse>> CreateCategory(
        [FromBody] CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Created("/api/admin/categories", response);
    }

    /// <summary>
    /// Deletar categoria
    /// </summary>
    [HttpDelete("categories/{categoryId}")]
    public async Task<ActionResult> DeleteCategory(
        [FromRoute] Guid categoryId, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteByIdCategoryRequest(categoryId), cancellationToken);
        return NoContent();
    }

    #endregion
}

// Classes temporárias para os requests
public class CreateCourseAdminRequest
{
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Difficulty { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public bool HaveExam { get; set; }
    public List<object> Modules { get; set; } = new();
}

public class CreateManagerRequest
{
    public string Name { get; set; } = string.Empty;
    public string Identity { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid CompanyId { get; set; }
}

public class UpdateCompanyRequest
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public UpdateCompanyRequest(Guid companyId, UpdateCompanyRequest request)
    {
        // Mapear as propriedades necessárias
        Name = request.Name;
        Address = request.Address;
        Phone = request.Phone;
    }
}

public class UpdateCompanyResponse
{
    public string Message { get; set; } = "Company updated successfully";
}
