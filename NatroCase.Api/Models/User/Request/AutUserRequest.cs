using NatroCase.Application.User.Commands;

namespace NatroCase.Api.Models.User.Request;

public record AutUserRequest(string Email, string Password)
{
    public AuthUserCommand ToCommand()
    {
        return new AuthUserCommand(Email, Password);
    }
}