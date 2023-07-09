using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using WebApi.Helpers;

namespace WebApi.Extensions
{
    public static class ExceptionHandler
    {
        public static void ConfigureException(this IApplicationBuilder app, IWebHostEnvironment hostEnvironment)
        {

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    IExceptionHandlerFeature? exceptionHandleFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandleFeature != null)
                    {
                        var status = 500;
                        switch (exceptionHandleFeature.Error)
                        {


                            case InvalidOperationException:
                            case InvalidDataException:
                            case KeyNotFoundException:
                            case ArgumentException:
                            

                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                status = 400;
                                break;
                            case DbUpdateException:
                                context.Response.StatusCode = StatusCodes.Status409Conflict;
                                status = 409;
                                break;
                            case UnauthorizedAccessException:
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                status = 403;
                                break;
                            default:
                                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                break;
                        }

                        ErrorResponse err = new ErrorResponse(exceptionHandleFeature.Error.Message);
                        
                        var serializerSettings = new JsonSerializerSettings();
                        serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        string msg = JsonConvert.SerializeObject(err, serializerSettings);
                        await context.Response.WriteAsync(msg);
                    }
                });
            });
        }
    }
}
