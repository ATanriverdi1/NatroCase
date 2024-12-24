using MediatR;
using Microsoft.EntityFrameworkCore;
using NatroCase.Application.Common.Interfaces;
using NatroCase.Application.User.Enums;
using NatroCase.Domain.Exceptions;
using NatroCase.Domain.User;

namespace NatroCase.Application.User.Queries;

public record UserByIdQuery(Guid Id) : IRequest<UserAggregate>
{
    public sealed class Handler : IRequestHandler<UserByIdQuery, UserAggregate>
    {
        private readonly INatroCaseDbContext _context;

        public Handler(INatroCaseDbContext context)
        {
            _context = context;
        }

        public async Task<UserAggregate> Handle(UserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (user == null)
                throw new NotFoundException(UserApplicationError.UserNotFoundWithGivenId,
                    new KeyValuePair<string, string>("id", request.Id.ToString()));
            
            return user;
        }
    }
}