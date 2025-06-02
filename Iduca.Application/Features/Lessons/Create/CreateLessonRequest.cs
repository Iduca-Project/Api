
using MediatR;

namespace Iduca.Application.Features.Lessons.Create;

public sealed record CreateLessonRequest(

) : IRequest<CreateLessonResponse>;
