using Iduca.Application.Repository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Application.Common.Exceptions;
using MediatR;

namespace Iduca.Application.Features.User.Delete;

public class DeleteUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteUserRequest>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.Get(request.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        userRepository.Delete(user);
        await unitOfWork.Save(cancellationToken);
    }
}
