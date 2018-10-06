using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Messaging;
using Android.Support.V4.App;
using System.Collections.Generic;
using Android.Support.V4.Content;

namespace BicyclesCecoApp.Droid
{
    [Activity(Label = "BicyclesCecoApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const int RequestSendBackgroundSms = 4;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            Button buttonSendBackgroundSms = FindViewById<Button>(Resource.Id.);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
             if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                SendBackgroundSms();
        }

        private void SendBackgroundSms()
        {
            var permissionChecks = new List<string>();
            if (ContextCompat.CheckSelfPermission(this, "android.permission.READ_PHONE_STATE") != Permission.Granted)
            {
                permissionChecks.Add("android.permission.READ_PHONE_STATE");
            }
            if (ContextCompat.CheckSelfPermission(this, "android.permission.SEND_SMS") != Permission.Granted)
            {
                permissionChecks.Add("android.permission.SEND_SMS");
            }

            if (permissionChecks.Count > 0)
            {
                ActivityCompat.RequestPermissions(this, permissionChecks.ToArray(), RequestSendBackgroundSms);
            }
            
        
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSms)
            {
                smsMessenger.SendSms("+34687612919", "Hello from your friends at Xamarin!");
            }
        }
    }
}