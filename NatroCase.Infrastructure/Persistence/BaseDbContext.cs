using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NatroCase.Domain.Common;

namespace NatroCase.Infrastructure.Persistence;

public abstract class BaseDbContext : DbContext
{
    
    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof (BaseDbContext).Assembly);
        this.ApplyAggregateProperties(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default (CancellationToken))
    {
        this.EnsureAggregateModifications();
        int num = await base.SaveChangesAsync(cancellationToken);
        return num;
    }

    private void EnsureAggregateModifications()
    {
        foreach (EntityEntry<AggregateRoot> entityEntry in this.ChangeTracker.Entries<AggregateRoot>().Where<EntityEntry<AggregateRoot>>((Func<EntityEntry<AggregateRoot>, bool>) (p => p.State != EntityState.Unchanged)).ToList<EntityEntry<AggregateRoot>>())
        {
            var entity = entityEntry.Entity;
            if (entity.IsModified) continue;
            if (entityEntry.State == EntityState.Added)
                entity.SetAsCreated();
            else
                entity.SetAsModified();
        }
    }
    
    private void ApplyAggregateProperties(ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType mutableEntityType in modelBuilder.Model.GetEntityTypes().Where<IMutableEntityType>((Func<IMutableEntityType, bool>) (t => typeof (AggregateRoot).IsAssignableFrom(t.ClrType))))
        {
            EntityTypeBuilder entityTypeBuilder = modelBuilder.Entity(mutableEntityType.ClrType);
            entityTypeBuilder.Ignore("Events");
            entityTypeBuilder.Ignore("IsModified");
            entityTypeBuilder.Property("Id").HasColumnName("id");
            entityTypeBuilder.Property("CreatedDate").HasColumnName("created_date");
            entityTypeBuilder.Property("LastModifiedDate").HasColumnName("last_modified_date");
            entityTypeBuilder.Property("Version").HasColumnName("xmin").HasColumnType("xid").ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();
            entityTypeBuilder.HasKey("Id");
        }
    }

}