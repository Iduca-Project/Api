using AutoMapper;
using Iduca.Application.Repository.UserRepository;
using MediatR;

namespace Iduca.Application.Features.User.GetByQuery;

public class GetUsersHandler(
    IUserRepository userRepository,
    IMapper mapper
) : IRequestHandler<GetUsersRequest, List<GetUsersResponse>>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IMapper mapper = mapper;

    public async Task<List<GetUsersResponse>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsersByQuery(
            request.Name,
            request.Email,
            request.CompanyId,
            request.IsAdmin,
            request.Page,
            request.MaxItems,
            cancellationToken
        );

        return mapper.Map<List<GetUsersResponse>>(users);
    }
}
