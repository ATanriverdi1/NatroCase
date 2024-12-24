namespace NatroCase.Application.User.Enums;

public static class UserApplicationError
{
    public static string UserAlreadyExistWithEmail => "USER_ALREADY_EXIST_WITH_EMAIL";
    public static string InvalidEmailOrPassword => "INVALID_EMAIL_OR_PASSWORD";
    public static string UserNotFoundWithGivenId => "USER_NOT_FOUND_WITH_GIVEN_ID";
}