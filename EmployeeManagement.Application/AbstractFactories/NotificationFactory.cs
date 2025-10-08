using EmployeeManagement.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDBFirst_Library.Factories
{
    public abstract class NotificationFactory
    {
        public abstract INotification CreateNotification(); 
    }
}
