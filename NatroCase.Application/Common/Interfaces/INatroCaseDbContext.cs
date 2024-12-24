using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NatroCase.Domain.User;

namespace NatroCase.Application.Common.Interfaces;

public interface INatroCaseDbContext
{
    public DbSet<UserAggregate> Users { get; set; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}