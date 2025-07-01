using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Application.Repository.CategoryRepository;
using Iduca.Domain.Models;
using Iduca.Domain.Common.Messages;
using Iduca.Application.Common.Exceptions;
using MediatR;

namespace Iduca.Application.Features.User.Update;

public class UpdateUserHandler(
    IUserRepository userRepository,
    ICompanyRepository companyRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserWithDetails(request.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        // Verificar se email já existe para outro usuário
        var existingUser = await userRepository.GetUserByEmail(request.Email, cancellationToken);
        if (existingUser is not null && existingUser.Id != request.Id)
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

        // Atualizar propriedades
        user.Name = request.Name;
        user.Identity = request.Identity;
        user.Email = request.Email;
        user.IsAdmin = request.IsAdmin;
        user.Responsible = responsible;
        user.ResponsibleId = request.ResponsibleId;
        user.Company = company;
        user.CompanyId = request.CompanyId;
        user.Image = request.Image;
        user.Interests = interests;
        user.UpdatedAt = DateTime.UtcNow;

        userRepository.Update(user);
        await unitOfWork.Save(cancellationToken);

        return mapper.Map<UpdateUserResponse>(user);
    }
}
