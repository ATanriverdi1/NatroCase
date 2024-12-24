using Microsoft.EntityFrameworkCore;
using NatroCase.Application.Common.Interfaces;

namespace NatroCase.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void DatabaseMigrate(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetService<INatroCaseDbContext>();
        context?.Database.Migrate();
    }
}