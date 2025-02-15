
using Microsoft.AspNetCore.Http;
using YGate.Entities;

namespace YGate.Server.Middlewares
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string IpAddress = context.Connection.RemoteIpAddress?.ToString();
            if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"].FirstOrDefault())) //CloudFlare kullanılıyorsa Ip Adresi X-Forwarded-For başlığında tutulur.
                IpAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            string? blockedIp = StaticTools.BlockedIp.SingleOrDefault(xd => xd == IpAddress);
            if (!string.IsNullOrEmpty(blockedIp))
            {
                // HTML yanıtı
                string htmlResponse = @"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Blocked IP</title>
            <style>
              body {
            font-family: Arial, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
            background-color: black;
        }

        .container {
            text-align: center;
            color: #333;
        }

        .skull {
            font-size: 16px;
            color: red;
            font-family: 'Consolas', 'Courier New', Courier, monospace;
            white-space: pre;
            line-height: 1.1;
            margin-bottom: 20px;
            display: inline-block;
            text-align: left;
        }

        .message {
            font-size: 24px;
            font-weight: bold;
            color: red;
        }
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='skull'>

          .                                                      .
        .n                   .                 .                  n.
  .   .dP                  dP                   9b                 9b.    .
 4    qXb         .       dX                     Xb       .        dXp     t
dX.    9Xb      .dXb    __                         __    dXb.     dXP     .Xb
9XXb._       _.dXXXXb dXXXXbo.                 .odXXXXb dXXXXb._       _.dXXP
 9XXXXXXXXXXXXXXXXXXXVXXXXXXXXOo.           .oOXXXXXXXXVXXXXXXXXXXXXXXXXXXXP
  `9XXXXXXXXXXXXXXXXXXXXX'~   ~`OOO8b   d8OOO'~   ~`XXXXXXXXXXXXXXXXXXXXXP'
    `9XXXXXXXXXXXP' `9XX'   DIE    `98v8P'  HUMAN   `XXP' `9XXXXXXXXXXXP'
        ~~~~~~~       9X.          .db|db.          .XP       ~~~~~~~
                        )b.  .dbo.dP'`v'`9b.odb.  .dX(
                      ,dXXXXXXXXXXXb     dXXXXXXXXXXXb.
                     dXXXXXXXXXXXP'   .   `9XXXXXXXXXXXb
                    dXXXXXXXXXXXXb   d|b   dXXXXXXXXXXXXb
                    9XXb'   `XXXXXb.dX|Xb.dXXXXX'   `dXXP
                     `'      9XXXXXX(   )XXXXXXP      `'
                              XXXX X.`v'.X XXXX
                              XP^X'`b   d'`X^XX
                              X. 9  `   '  P )X
                              `b  `       '  d'
                               `             '
                </div>
                <div class='message'>
                    I've blocked the IP address, Bozok, maybe one day.
                </div>
            </div>
        </body>
        </html>";

                // Yanıt olarak HTML döndür
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(htmlResponse);
                return;
            }

            // tüm controller isteklerimi post olarak yapıyorum.
            // bu nedenle kayıt işlemini, istek tipi post ise yapmakta makul olur.
            if (context.Request.Method == "POST")
            {
                StaticTools.AddRequest(IpAddress);
            }




            await _next(context);
        }
    }
}
