using EmployeeManagement.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDBFirst_Library.Channel
{
    public  class EmailNotification :INotification
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"Email notification is - {message}");
        }
    }
}
