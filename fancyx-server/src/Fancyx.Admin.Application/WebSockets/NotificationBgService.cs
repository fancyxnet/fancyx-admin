using Fancyx.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Channels;

namespace Fancyx.Admin.Application.WebSockets;

public class NotificationBgService : BackgroundService
{
    private readonly ChannelReader<NotificationMessage> _channelReader;
    private readonly WebSocketConnectionManager _connectionManager;
    private readonly ILogger<NotificationBgService> _logger;

    public NotificationBgService(
        WebSocketConnectionManager webSocketConnectionManager,
        ChannelReader<NotificationMessage> channelReader,
        ILogger<NotificationBgService> logger)
    {
        _connectionManager = webSocketConnectionManager;
        _channelReader = channelReader;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _channelReader.ReadAllAsync(stoppingToken))
        {
            try
            {
                if (_connectionManager.TryGetWebSocket(message.UserId, out var ws) &&
                    ws.State == WebSocketState.Open)
                {
                    var payload = JsonUtils.Serialize(message);

                    var bytes = Encoding.UTF8.GetBytes(payload);
                    await ws.SendAsync(
                        new ArraySegment<byte>(bytes),
                        WebSocketMessageType.Text,
                        true,
                        stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification to user {UserId}", message.UserId);
            }
        }
    }
}
