
using MediatR;

namespace Iduca.Application.Features.LessonUsers.Create;

public sealed record CreateLessonuserRequest(

) : IRequest<CreateLessonuserResponse>;
