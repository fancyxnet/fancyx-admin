using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Fancyx.Admin.Application.WebSockets;

public class WebSocketConnectionManager : IDisposable
{
    private readonly ConcurrentDictionary<long, WebSocket> _connections = new();
    private readonly ILogger<WebSocketConnectionManager> _logger;

    public WebSocketConnectionManager(ILogger<WebSocketConnectionManager> logger)
    {
        _logger = logger;
    }

    public void AddConnection(long userId, WebSocket webSocket)
    {
        _connections.AddOrUpdate(userId, webSocket, (key, existing) =>
        {
            // 如果已有连接，先关闭旧的
            _ = CloseAsync(existing, "Replaced by new connection");
            return webSocket;
        });
        _logger.LogInformation("User {UserId} connected", userId);
    }

    public bool TryGetWebSocket(long userId, out WebSocket ws)
    {
        return _connections.TryGetValue(userId, out ws!);
    }

    public async Task CloseAsync(WebSocket ws, string reason)
    {
        if (ws.State == WebSocketState.Open || ws.State == WebSocketState.CloseReceived)
        {
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, reason, CancellationToken.None);
        }
    }

    public async Task RemoveConnection(long userId)
    {
        if (_connections.TryRemove(userId, out var ws))
        {
            await CloseAsync(ws, "Client disconnected");
        }
    }

    public IEnumerable<long> GetAllUserIds() => _connections.Keys;

    public void Dispose()
    {
        foreach (var (_, ws) in _connections)
        {
            _ = CloseAsync(ws, "Server shutting down");
        }
    }
}
