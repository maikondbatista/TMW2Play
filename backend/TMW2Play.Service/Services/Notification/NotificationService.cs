using TMW2Play.Service.Domain.Services;

namespace TMW2Play.Service.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private List<string> _notifications = new List<string>();
        public void AddNotification(string notification)
        {
            _notifications.Add(notification);
        }

        public List<string> Notifications()
        {
            var errors = new List<string>();
            errors.AddRange(_notifications);

            return errors;
        }
    }
}
