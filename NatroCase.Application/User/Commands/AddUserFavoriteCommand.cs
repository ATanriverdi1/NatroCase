using MediatR;
using NatroCase.Application.Common.Interfaces;
using NatroCase.Application.User.Queries;
using NatroCase.Domain.User.Entities;

namespace NatroCase.Application.User.Commands;

public record AddUserFavoriteCommand(Guid UserId, string DomainName, bool IsAvailable) : IRequest
{
    public sealed class Handler : IRequestHandler<AddUserFavoriteCommand>
    {
        private readonly IMediator _mediator;
        private readonly INatroCaseDbContext _context;

        public Handler(IMediator mediator, INatroCaseDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Unit> Handle(AddUserFavoriteCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new UserByIdQuery(request.UserId), cancellationToken);
            if (user.Favorites.Any(f => f.DomainName == request.DomainName))
                return Unit.Value;
            
            user.AddFavorite(request.DomainName, request.IsAvailable);
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}