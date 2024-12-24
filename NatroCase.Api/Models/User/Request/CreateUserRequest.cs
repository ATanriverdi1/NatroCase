using NatroCase.Application.User.Commands;

namespace NatroCase.Api.Models.User.Request;

public record CreateUserRequest(string Email, string Name, string Password)
{
    public CreateUserCommand ToCommand()
    {
        return new CreateUserCommand(Email, Name, Password);
    }
}