using AutoMapper;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository;
using MediatR;

namespace Iduca.Application.Features.Auth.CheckCode;

public class CheckCodeHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<CheckCodeRequest, CheckCodeResponse>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<CheckCodeResponse> Handle(CheckCodeRequest request, CancellationToken cancellationToken)
    {
        // TODO: Implementar verificação do código no banco de dados
        // Por enquanto, vamos aceitar qualquer código de 5 dígitos para desenvolvimento
        
        if (request.Code.Length == 5 && int.TryParse(request.Code, out _))
        {
            return new CheckCodeResponse(true);
        }

        return new CheckCodeResponse(false);
    }
}
