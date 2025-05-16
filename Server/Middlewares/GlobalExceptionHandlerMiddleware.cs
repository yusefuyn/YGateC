using System.Net;
using YGate.Entities;
using YGate.Interfaces.OperationLayer;

namespace YGate.Server.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {

        private readonly RequestDelegate _next;
        IJsonSerializer jsonSerializer;


        public GlobalExceptionHandlerMiddleware(RequestDelegate next, IJsonSerializer jsonSerializer)
        {
            _next = next;
            this.jsonSerializer = jsonSerializer;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Diğer middleware'leri çalıştır
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception occurred {ex}");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = jsonSerializer.Serialize(new RequestResult("Hata Meydana Geldi")
                {
                    ShortDescription = "Bir hata meydana geldi"
                });

                await context.Response.WriteAsync(result);
            }
        }

    }
}
