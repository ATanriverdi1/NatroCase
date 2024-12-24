using System.Data;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using NatroCase.Application.Common.Models;
using NatroCase.Domain.Exceptions;
using Newtonsoft.Json;

namespace NatroCase.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next) => this._next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        this._logger = httpContext.RequestServices.GetRequiredService<ILogger<ExceptionMiddleware>>();
        await this._next(httpContext);
      }
      catch (Exception ex)
      {
        await this.HandleExceptionAsync(httpContext, ex);
      }
    }

    protected virtual async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
      Type exType = ex.GetType();
      object? content = null;
      Type type = exType;
      HttpStatusCode statusCode;
      NotFoundException notFoundException;
      ErrorResponse errorResponse;
      if ((object) type != null)
      {
        if (exType == typeof (NotFoundException))
        {
          statusCode = HttpStatusCode.NotFound;
          notFoundException = ex as NotFoundException;
          if (!string.IsNullOrEmpty(notFoundException.Key))
          {
            content = new ErrorResponse(notFoundException);
          }

          goto label_13;
        }
      }
      statusCode = HttpStatusCode.InternalServerError;
      this._logger.LogCritical(new EventId(0, "UNKNOWN_EXCEPTION"), ex, "unknown exception: " + ex.Message);
label_13:
      notFoundException = (NotFoundException) null;
      errorResponse = (ErrorResponse) null;
      type = (Type) null;
      httpContext.Response.StatusCode = statusCode.GetHashCode();
      if (content != null)
        await httpContext.Response.WriteAsJsonAsync<object>(content);
      await httpContext.Response.CompleteAsync();
      exType = (Type) null;
      content = (object) null;
    }
}