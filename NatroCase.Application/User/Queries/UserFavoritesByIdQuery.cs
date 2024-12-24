using MediatR;
using NatroCase.Application.Common.Interfaces;
using NatroCase.Application.Common.Models;
using NatroCase.Domain.User.Entities;

namespace NatroCase.Application.User.Queries;

public record UserFavoritesByIdQuery(Guid UserId, string? DomainName, int PageIndex, int PageSize) : IRequest<Nextable<Favorite>>
{
    public sealed class Handler : IRequestHandler<UserFavoritesByIdQuery, Nextable<Favorite>>
    {
        private readonly IMediator _mediator;
        private readonly INatroCaseDbContext _context;

        public Handler(IMediator mediator, INatroCaseDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Nextable<Favorite>> Handle(UserFavoritesByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new UserByIdQuery(request.UserId), cancellationToken);
            var favorites = user.Favorites;
            if (!string.IsNullOrWhiteSpace(request.DomainName))
            {
                favorites = favorites.Where(x => x.DomainName.ToLower().Contains(request.DomainName.ToLower())).ToList();
            }
            
            var contents = favorites
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize + 1)
                .ToList();
            
            return new Nextable<Favorite>(contents.Count > request.PageSize, contents);
        }
    }
}