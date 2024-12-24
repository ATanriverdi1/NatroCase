using NatroCase.Application.User.Commands;

namespace NatroCase.Api.Models.User.Request;

public record RemoveUserFavoriteRequest(string DomainName)
{
    public RemoveUserFavoriteCommand ToCommand(Guid id)
    {
        return new RemoveUserFavoriteCommand(id, DomainName);
    }
}