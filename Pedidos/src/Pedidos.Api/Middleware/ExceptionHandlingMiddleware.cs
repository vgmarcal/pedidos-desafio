using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Pedidos.Api.Middleware
{
    /// <summary>
    /// Captura exceções não tratadas do pipeline e converte em resposta JSON padronizada,
    /// papel parecido com o Application_Error / ExceptionFilterAttribute do ASP.NET clássico.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                // Validações de domínio lançam ArgumentException e viram 400 (Bad Request).
                _logger.LogWarning(ex, "Requisição inválida");
                await WriteProblemAsync(context, HttpStatusCode.BadRequest, "Requisição inválida", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado");
                await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "Erro interno", "Ocorreu um erro inesperado.");
            }
        }

        private static Task WriteProblemAsync(HttpContext context, HttpStatusCode statusCode, string title, string detail)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            var problem = new
            {
                status = (int)statusCode,
                title = title,
                detail = detail
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
