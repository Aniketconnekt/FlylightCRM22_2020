using CRM.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.View.Admin.NotificationPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        public string deviceToken = "cct6m4OAO3Q:APA91bE2kLYKmeqbH48uSO340BE2gYeU3Ky1-uQq_p7b9hfeTX0BfwVLh9ukrNwpYfD8W7gq8Y_IaStCV_CsGHmdcQjXD7bAEuuNDvHAAVM5Vttt7SCD3CT3t6FM4oLoH9XpPefJM-Di";
        public NotificationPage()
        {
            InitializeComponent();
        }
        private void SendNotification(object sender, EventArgs e)
        {
            // DependencyService.Get<INotification>().CreateNotification("FlylightCRM", message.Text);
            bool CreateNotification(string deviceToken, string message)
            {
                //TODO:Send Push Notification On Multiple devices..
                //List<string> deviceToken = new List<string>();
                //deviceToken.Add("cgAYD30kCMw:APA91bEkBnSVjH6qekzazihDzh-0cTuumlD0Q9IFjwxypZldbCN-SURtS7pTbeSZ1e-Z1IeReFMYzi4VDGDGV-hzCuR92oI4tYoM8PEUY2yaXt8L-pummgTZOWcrQIcO503wx3jfpbSW");
                //deviceToken.Add("cgAYD30kCMw:APA91bEkBnSVjH6qekzazihDzh-0cTuumlD0Q9IFjwxypZldbCN-SURtS7pTbeSZ1e-Z1IeReFMYzi4VDGDGV-hzCuR92oI4tYoM8PEUY2yaXt8L-pummgTZOWcrQIcO503wx3jfpbSW");
                // code of push notification


                string applicationID = "AAAAGudpe3c:APA91bFfkbUY8yDmwELyuSfV3f - FTpwry5HGQen1c_IKf4ozeBvAjE1avZTz5glTutBfn6jDRr1QSDB4JG6BWZx7lYkNv7WCdv4Rj2nXFjIHRrIbel0si5_2wt1xXlUnSSrSgjJRomzr";
                //string applicationID = "AAAA55p-aJQ:APA91bFpT36CmtLjQi5jrcrj9IU4dZZF8VI5FMPyOANnNAb_s10dSOvltikwEkKtJO25gfWlL-4YIJbK418A2HMu5Ie9kyIHAqLtLipWZcfcoPQ2L8rP0K8SqKlvWBdK3zp3g9-b27BF";
                string senderId = "115551599479";
                string deviceId = string.Join("\",\"", deviceToken);
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = message,
                        title = "FlylightCRM",
                        sound = "Enabled",
                    },

                };
                string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                Byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(jsonNotificationFormat);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (System.IO.Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (System.IO.Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
                return true;
            }
        }
    }
}