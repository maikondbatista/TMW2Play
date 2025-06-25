namespace TMW2Play.Service.Domain.Services
{
    public interface INotificationService
    {
        void AddNotification(string notification);
        List<string> Notifications();
    }
}
