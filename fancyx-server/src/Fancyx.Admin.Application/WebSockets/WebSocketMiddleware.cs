using Fancyx.Admin.Application.SharedService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace Fancyx.Admin.Application.WebSockets;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;

    public WebSocketMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path != "/ws")
        {
            await _next(context);
            return;
        }

        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        // 获取用户ID
        var requestToken = context.Request.Query["token"].ToString()?.Trim();
        if (string.IsNullOrEmpty(requestToken))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
        var identityService = context.RequestServices.GetRequiredService<IdentitySharedService>();
        var claimsPrincipal = identityService.GetPrincipalFromAccessToken(requestToken);
        if (claimsPrincipal == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
        var isSucc = long.TryParse(claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out var userId);
        if (!isSucc || userId <= 0)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var manager = context.RequestServices.GetRequiredService<WebSocketConnectionManager>();

        manager.AddConnection(userId, webSocket);

        // 启动两个并行任务：接收 & 心跳
        var receiveTask = ReceiveLoop(webSocket, manager, userId);
        var pingTask = PingLoop(webSocket, manager, userId);

        await Task.WhenAny(receiveTask, pingTask);

        await _next(context);
    }

    private static async Task ReceiveLoop(WebSocket ws, WebSocketConnectionManager manager, long userId)
    {
        var buffer = new byte[1024];
        try
        {
            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }
            }
        }
        finally
        {
            await manager.RemoveConnection(userId);
        }
    }

    private static async Task PingLoop(WebSocket ws, WebSocketConnectionManager manager, long userId)
    {
        try
        {
            while (ws.State == WebSocketState.Open)
            {
                await Task.Delay(TimeSpan.FromSeconds(25), CancellationToken.None); // 每25秒 ping

                if (ws.State != WebSocketState.Open) break;

                var pingMsg = JsonConvert.SerializeObject(new { type = "ping", timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() });
                var bytes = Encoding.UTF8.GetBytes(pingMsg);
                await ws.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
        }
        catch { }
    }
}
