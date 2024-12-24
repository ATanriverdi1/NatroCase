using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NatroCase.Application.Common.Interfaces;
using NatroCase.Domain.User;

namespace NatroCase.Infrastructure.Persistence;

public class NatroCaseDbContext : BaseDbContext, INatroCaseDbContext
{
    public NatroCaseDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NatroCaseDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<UserAggregate> Users { get; set; }
    
    public DatabaseFacade Database => base.Database;
    
}