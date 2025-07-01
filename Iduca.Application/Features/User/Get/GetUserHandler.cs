using AutoMapper;
using Iduca.Application.Repository.UserRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Application.Common.Exceptions;
using MediatR;

namespace Iduca.Application.Features.User.Get;

public class GetUserHandler(
    IUserRepository userRepository,
    IMapper mapper
) : IRequestHandler<GetUserRequest, GetUserResponse>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserWithDetails(request.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        return mapper.Map<GetUserResponse>(user);
    }
}
