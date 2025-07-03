using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository;
using MediatR;

namespace Iduca.Application.Features.Auth.ResetPassword;

public class ResetPasswordHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<ResetPasswordRequest, ResetPasswordResponse>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<ResetPasswordResponse> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        // Verificar se o usuário existe
        var user = await userRepository.GetUserByEmail(request.Email, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        // TODO: Implementar hash da senha antes de salvar
        user.Password = request.NewPassword;
        
        userRepository.Update(user);
        await unitOfWork.Save(cancellationToken);

        return new ResetPasswordResponse(true);
    }
}
