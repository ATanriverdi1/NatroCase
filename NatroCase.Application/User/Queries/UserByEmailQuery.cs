using MediatR;
using Microsoft.EntityFrameworkCore;
using NatroCase.Application.Common.Interfaces;
using NatroCase.Domain.User;

namespace NatroCase.Application.User.Queries;

public record UserByEmailQuery(string Email) : IRequest<UserAggregate?>
{
    public sealed class Handler : IRequestHandler<UserByEmailQuery, UserAggregate?>
    {
        private readonly INatroCaseDbContext _context;

        public Handler(INatroCaseDbContext context)
        {
            _context = context;
        }

        public async Task<UserAggregate?> Handle(UserByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email == request.Email, cancellationToken);
        }
    }
}