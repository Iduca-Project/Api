using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Application.Repository.CategoryRepository;
using Iduca.Domain.Models;
using Iduca.Domain.Common.Messages;
using Iduca.Application.Common.Exceptions;
using MediatR;
using BC = BCrypt.Net.BCrypt;

namespace Iduca.Application.Features.User.Create;

public class CreateUserHandler(
    IUserRepository userRepository,
    ICompanyRepository companyRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        // Verificar se já existe usuário com mesmo email
        var existingUser = await userRepository.GetUserByEmail(request.Email, cancellationToken);
        if (existingUser is not null)
            throw new DuplicityException("Já existe um usuário com este email.");

        // Verificar se a empresa existe
        var company = await companyRepository.Get(request.CompanyId, cancellationToken)
            ?? throw new NotFoundException("Empresa não encontrada.");

        // Verificar se o responsável existe (se informado)
        Domain.Models.User? responsible = null;
        if (request.ResponsibleId.HasValue)
        {
            responsible = await userRepository.Get(request.ResponsibleId.Value, cancellationToken)
                ?? throw new NotFoundException("Responsável não encontrado.");
        }

        // Buscar categorias de interesse
        var interests = new List<Category>();
        foreach (var interestId in request.Interests)
        {
            var category = await categoryRepository.Get(interestId, cancellationToken)
                ?? throw new NotFoundException($"Categoria {interestId} não encontrada.");
            interests.Add(category);
        }

        var user = new Domain.Models.User
        {
            Name = request.Name,
            Identity = request.Identity,
            Email = request.Email,
            Password = BC.HashPassword(request.Password),
            IsAdmin = request.IsAdmin,
            Responsible = responsible,
            ResponsibleId = request.ResponsibleId,
            Company = company,
            CompanyId = request.CompanyId,
            Image = request.Image,
            Interests = interests,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        userRepository.Create(user);
        await unitOfWork.Save(cancellationToken);

        return mapper.Map<CreateUserResponse>(user);
    }
}
