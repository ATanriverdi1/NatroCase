using MediatR;
using NatroCase.Application.Common.Interfaces;
using NatroCase.Domain.User;
using Microsoft.Extensions.Configuration;
using NatroCase.Application.Common.Constants;
using NatroCase.Application.User.Enums;
using NatroCase.Application.User.Queries;
using NatroCase.Domain.Exceptions;

namespace NatroCase.Application.User.Commands;

public record CreateUserCommand(string Email, string Name, string Password) : IRequest
{
    public sealed class Handler : IRequestHandler<CreateUserCommand>
    {
        private readonly INatroCaseDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public Handler(INatroCaseDbContext context, IConfiguration configuration, IMediator mediator)
        {
            _context = context;
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new UserByEmailQuery(request.Email), cancellationToken);
            if (user != null)
                throw new BusinessException(UserApplicationError.UserAlreadyExistWithEmail,
                    new KeyValuePair<string, string>("email", request.Email));
            
            var saltKey = _configuration[ApplicationConfigKeys.SaltKey];
            var password = UserAggregate.GeneratePassword(request.Password, saltKey);
            user = UserAggregate.Create(
                request.Email,
                request.Name,
                password);
            
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}