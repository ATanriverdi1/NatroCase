using NatroCase.Application.User.Commands;

namespace NatroCase.Api.Models.User.Request;

public record AddUserFavoriteRequest(string DomainName, bool IsAvailable)
{
    public AddUserFavoriteCommand ToCommand(Guid id)
    {
        return new AddUserFavoriteCommand(id, DomainName, IsAvailable);
    }
}