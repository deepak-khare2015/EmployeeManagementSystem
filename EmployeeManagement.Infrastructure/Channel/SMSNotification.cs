using EmployeeManagement.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDBFirst_Library.Channel
{
    public   class SMSNotification :INotification
    {
        public void SendMessage (string message)
        {

            Console.WriteLine ($"SMS notification is  - {message}");
        }
    }
}
