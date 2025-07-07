using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Application.Repository.CategoryRepository;
using Iduca.Application.Common.Services;
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
    IMapper mapper,
    ILogService logService
) : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;
    private readonly ILogService logService = logService;

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        // Verificar se já existe usuário com mesmo email
        var existingUser = await userRepository.GetUserByEmail(request.Email, cancellationToken);
        if (existingUser is not null)
            throw new DuplicityException("Já existe um usuário com este email.");


        // Verificar se o responsável existe (se informado)
        Domain.Models.User responsible =  await userRepository.Get((Guid)request.ResponsibleId!, cancellationToken)
                ?? throw new NotFoundException("Responsável não encontrado.");

        var companyId = request.CompanyId == null ? responsible.CompanyId : (Guid)request.CompanyId;

        // Verificar se a empresa existe
        var company = await companyRepository.Get(companyId , cancellationToken)
            ?? throw new NotFoundException("Empresa não encontrada.");
        

        // Buscar categorias de interesse (se informadas)
        var interests = new List<Category>();
        if (request.Interests != null)
        {
            foreach (var interestId in request.Interests)
            {
                var category = await categoryRepository.Get(interestId, cancellationToken)
                    ?? throw new NotFoundException($"Categoria {interestId} não encontrada.");
                interests.Add(category);
            }
        }

        Console.WriteLine("\n\n\n");
        Console.WriteLine(request.CompanyId == null ? responsible.CompanyId : (Guid)request.CompanyId);
        Console.WriteLine("\n\n\n");


        var user = new Domain.Models.User
        {
            Name = request.Name,
            Identity = request.Identity,
            Email = request.Email,
            Password = BC.HashPassword(request.Password),
            IsAdmin = request.IsAdmin ?? false,
            Responsible = responsible,
            ResponsibleId = request.ResponsibleId,
            Company = company,
            CompanyId = companyId,
            Image = request.Image,
            Interests = interests,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        userRepository.Create(user);
        await unitOfWork.Save(cancellationToken);

        // Log da criação do usuário
        await logService.LogCreateAsync("User", user.Id, user.Id, cancellationToken);

        return mapper.Map<CreateUserResponse>(user);
    }
}
