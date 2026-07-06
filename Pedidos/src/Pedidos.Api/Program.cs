using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Pedidos.Api.Middleware;
using Pedidos.Application;
using Pedidos.Infrastructure;
using Pedidos.Infrastructure.Data;

namespace Pedidos.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Registro dos serviços no container de injeção de dependência.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pedidos API",
                    Version = "v1",
                    Description = "API para gerenciamento de pedidos - Desafio Stefanini"
                });
            });

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Pipeline de requisição (equivalente aos handlers/módulos do ASP.NET clássico).
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pedidos API v1");
            });

            // Aplica as migrations pendentes no start quando estiver usando SQL Server.
            if (!builder.Configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    dbContext.Database.Migrate();
                }
            }

            app.UseHttpsRedirection();

            app.UseCors("Frontend");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
