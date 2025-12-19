namespace Fancyx.Admin.Application.WebSockets
{
    public record NotificationMessage(long UserId, string Title, string Content, int NoReadedCount);
}
