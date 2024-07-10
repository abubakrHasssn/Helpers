using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.WebApi.Common.DTOs.InfrastructureDTOs;
using Demo.Domain.Abstract.Entities;

namespace Demo.WebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IloggerService _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, IloggerService logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                ErrorHandling error = new ErrorHandling
                {
                    StatusCode = context.Response.StatusCode.ToString(),
                    ErrorMessage = ex.Message
                };

                await context.Response.WriteAsync(error.ToString());
            }
        }
    }
}
