namespace Iduca.Domain.Common.Messages;

public static class ExceptionMessage
{
    public static class BadRequest
    {
        public const string Default = "Bad request";
    }

    public static class DuplicityModel
    {
        public const string Default = "It's already exists!";
        public const string EmailUserDuplicity = "An user with that e-mail already exists!";
        public const string CompanyNameDuplicity = "An company with that name already exists!";
        public const string CategoryNameDuplicity = "An category with that name already exists!";
        public const string ModuleNameDuplicity = "An module with that name already exists!";
        public const string LessonNameDuplicity = "An lesson with that name already exists!";
    }

    public static class InternalServerError
    {
        public const string Default = "Internal server error";
    }

    public static class NotFound
    {
        public const string Default = "Item not found";
        public const string CourseIdNotFound = "A course with that Id does not exist";
    }

    public static class Unauthorized
    {
        public const string Default = "Unauthorized.";
        public const string Session = "Invalid user session, you must login first.";
        public const string Token = "Invalid bearer token provided in header.";
        public const string TokenPrefix = "Token must be Bearer type.";
        public const string Credentials = "Credentials do not match or incorrect password.";
    }
}