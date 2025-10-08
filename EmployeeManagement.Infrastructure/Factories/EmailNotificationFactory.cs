using EmployeeDBFirst_Library.Channel;
using EmployeeManagement.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EmployeeDBFirst_Library.Factories
{
    public class EmailNotificationFactory :NotificationFactory
    {
        public override INotification CreateNotification()
        {
            return new EmailNotification();
        }
    }
}
