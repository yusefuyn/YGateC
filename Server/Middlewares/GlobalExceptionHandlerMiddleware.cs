using System.Net;

namespace YGate.Server.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {

        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
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

                var result = JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.",
                    detail = ex.Message // İstersen burayı production'da kapatabilirsin
                });

                await context.Response.WriteAsync(result);
            }
        }

    }
}
