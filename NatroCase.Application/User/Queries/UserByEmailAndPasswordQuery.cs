using MediatR;
using Microsoft.EntityFrameworkCore;
using NatroCase.Application.Common.Interfaces;
using NatroCase.Application.User.Enums;
using NatroCase.Domain.Exceptions;
using NatroCase.Domain.User;

namespace NatroCase.Application.User.Queries;

public record UserByEmailAndPasswordQuery(string Email, string Password) : IRequest<UserAggregate>
{
    public sealed class Handler : IRequestHandler<UserByEmailAndPasswordQuery, UserAggregate>
    {
        private readonly INatroCaseDbContext _context;

        public Handler(INatroCaseDbContext context)
        {
            _context = context;
        }

        public async Task<UserAggregate> Handle(UserByEmailAndPasswordQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email == request.Email && p.Password == request.Password,
                    cancellationToken);
            
            if (user == null)
                throw new BusinessException(UserApplicationError.InvalidEmailOrPassword,
                    new KeyValuePair<string, string>("email", request.Email));

            return user;
        }
    }
}