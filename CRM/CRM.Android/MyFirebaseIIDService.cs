﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;

namespace CRM.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {

        const string TAG = "MyFirebaseIIDService";

        public string DeviceToken;


        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            SendRegistrationToServer(refreshedToken);
            DeviceToken = refreshedToken;
        }
        void SendRegistrationToServer(string token)
        {
            // send this token to server
        }
    }
}