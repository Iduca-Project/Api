using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Auth.ForgotPassword;

public class ForgotPasswordHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<ForgotPasswordRequest, ForgotPasswordResponse>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<ForgotPasswordResponse> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        // Verificar se o usuário existe
        var user = await userRepository.GetUserByEmail(request.Email, cancellationToken);
        if (user == null)
        {
            // Por segurança, retornamos true mesmo se o usuário não existir
            // para não dar informações sobre emails válidos
            return new ForgotPasswordResponse(true);
        }

        // Gerar código de 5 dígitos
        var random = new Random();
        var code = random.Next(10000, 99999).ToString();

        // TODO: Salvar o código no banco de dados com expiração
        // TODO: Implementar envio de email com o código
        
        // Por enquanto, vamos apenas logar o código para desenvolvimento
        Console.WriteLine($"Código de recuperação para {request.Email}: {code}");

        return new ForgotPasswordResponse(true);
    }
}
