using MediatR;
using NatroCase.Application.Common.Interfaces;
using NatroCase.Application.User.Queries;

namespace NatroCase.Application.User.Commands;

public record RemoveUserFavoriteCommand(Guid UserId, string DomainName) : IRequest
{
    public sealed class Handler : IRequestHandler<RemoveUserFavoriteCommand>
    {
        private readonly IMediator _mediator;
        private readonly INatroCaseDbContext _context;

        public Handler(INatroCaseDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveUserFavoriteCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new UserByIdQuery(request.UserId), cancellationToken);
            if (user.Favorites.All(f => f.DomainName != request.DomainName))
                return Unit.Value;
            
            user.RemoveFavorite(request.DomainName);
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}