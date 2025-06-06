namespace TMW2Play.Service.Services.Notification
{
    public interface INotificationService
    {
        void AddNotification(string notification);
        List<string> Notifications();
    }
}
