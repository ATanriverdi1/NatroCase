using MediatR;
using Microsoft.Extensions.Configuration;
using NatroCase.Application.Common.Constants;
using NatroCase.Application.User.Queries;
using NatroCase.Domain.User;

namespace NatroCase.Application.User.Commands;

public record AuthUserCommand(string Email, string Password) : IRequest<UserAuthToken>
{
    public sealed class Handler : IRequestHandler<AuthUserCommand, UserAuthToken>
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public Handler(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<UserAuthToken> Handle(AuthUserCommand request, CancellationToken cancellationToken)
        {
            var saltKey = _configuration[ApplicationConfigKeys.SaltKey] ?? string.Empty;
            var secretKey = _configuration[ApplicationConfigKeys.SecretKey] ?? string.Empty;
            var expiresInMinutes = int.Parse(_configuration[ApplicationConfigKeys.ExpiresInMinutes] ?? "500");
            var password = UserAggregate.GeneratePassword(request.Password, saltKey);
            var user = await _mediator.Send(new UserByEmailAndPasswordQuery(request.Email, password), cancellationToken);
            var userAuthToken = user.Authorize(secretKey, expiresInMinutes);
            return userAuthToken;
        }
    }
}