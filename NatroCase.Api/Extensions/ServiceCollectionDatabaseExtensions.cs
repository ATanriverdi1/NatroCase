using Microsoft.EntityFrameworkCore;
using NatroCase.Api.Configuration;
using NatroCase.Application.Common.Interfaces;
using NatroCase.Infrastructure.Persistence;
using Npgsql;

namespace NatroCase.Api.Extensions;

public static class ServiceCollectionDatabaseExtensions
{
    public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<INatroCaseDbContext, NatroCaseDbContext>(p =>
            p.UseNpgsql(
                configuration.GetValue<string>(ConfigKeys.DatabaseConnection),
                options =>
                {
                    options.MigrationsAssembly(typeof(NatroCaseDbContext).Assembly.FullName);
                    NpgsqlConnection.GlobalTypeMapper.UseJsonNet();
                }));

        return services;
    }
}