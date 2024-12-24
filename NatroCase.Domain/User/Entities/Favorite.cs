namespace NatroCase.Domain.User.Entities;

public record Favorite(string DomainName, bool IsAvailable, long LastChecked);