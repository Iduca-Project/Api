using AutoMapper;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository;
using MediatR;

namespace Iduca.Application.Features.Auth.ResendCode;

public class ResendCodeHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<ResendCodeRequest, ResendCodeResponse>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<ResendCodeResponse> Handle(ResendCodeRequest request, CancellationToken cancellationToken)
    {
        // Verificar se o usuário existe
        var user = await userRepository.GetUserByEmail(request.Email, cancellationToken);
        if (user == null)
        {
            // Por segurança, retornamos true mesmo se o usuário não existir
            return new ResendCodeResponse(true);
        }

        // Gerar novo código de 5 dígitos
        var random = new Random();
        var code = random.Next(10000, 99999).ToString();

        // TODO: Salvar o novo código no banco de dados
        // TODO: Implementar envio de email com o novo código
        
        Console.WriteLine($"Novo código de recuperação para {request.Email}: {code}");

        return new ResendCodeResponse(true);
    }
}
