namespace NatroCase.Application.External.Rdap.Models.Response;

public record CheckDomainNameResponse(bool IsAvailable, string DomainName);