using System.Text.Json.Serialization;
using CorrelationId;
using MediatR;
using Microsoft.OpenApi.Models;
using NatroCase.Api.Extensions;
using NatroCase.Api.Middlewares;
using NatroCase.Application.Common.Interfaces;

namespace NatroCase.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            
            services.AddDatabases(Configuration);
            services.AddCorrelationId();
            services.AddHttpClients(Configuration);
            services.AddHttpContextAccessor();
            services.AddSwagger();
            services.AddMediatR(typeof(INatroCaseDbContext).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, INatroCaseDbContext dbContext)
        {
            app.UseCorrelationId(new CorrelationIdOptions
            {
                Header = "x-correlationid",
                UseGuidForCorrelationId = true,
                UpdateTraceIdentifier = true
            });
            
            app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NatroCase Api v1"));


            app.UseMiddleware<AuthMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.DatabaseMigrate();
        }
    }
}