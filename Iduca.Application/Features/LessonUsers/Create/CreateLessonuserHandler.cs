
using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.LessonuserRepository;
using MediatR;

namespace Iduca.Application.Features.LessonUsers.Create;

public class CreateLessonuser(
    IUnitOfWork unitOfWork,
    ILessonuserRepository lessonuserRepository,
    IMapper mapper
) : IRequestHandler<CreateLessonuserRequest, CreateLessonuserResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly ILessonuserRepository lessonuserRepository = lessonuserRepository;
    private readonly IMapper mapper = mapper;

    public async Task<CreateLessonuserResponse> Handle(CreateLessonuserRequest request, CancellationToken cancellationToken)
    {

        await unitOfWork.Save(cancellationToken);
        return mapper.Map<CreateLessonuserResponse>(lessonuser);
    }
}
