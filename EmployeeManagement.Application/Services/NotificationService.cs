using EmployeeDBFirst_Library.Factories;
using EmployeeManagement.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services
{
    public class NotificationService :INotificationService
    {
        private readonly NotificationFactory _notificationFactory;

        public NotificationService(NotificationFactory notificationFactory)
        {
            _notificationFactory = notificationFactory;
        }

        public void SendNotification(string message)
        {
            var notification = _notificationFactory.CreateNotification();
            notification.SendMessage (message);

        }
    }
}
