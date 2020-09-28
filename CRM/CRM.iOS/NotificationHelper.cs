using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Interfaces;
using CRM.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationHelper))]
namespace CRM.iOS
{
    public class NotificationHelper: INotification
    {
        public void CreateNotification(string title, string message)
        {
            new NotificationDelegate().RegisterNotification(title, message);
        }
    }
}