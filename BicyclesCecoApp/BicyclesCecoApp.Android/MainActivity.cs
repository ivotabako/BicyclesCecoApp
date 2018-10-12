
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Essentials;
using Android.Content;

namespace BicyclesCecoApp.Droid
{
    [Activity(Label = "BicyclesCecoApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        readonly string[] Permissions =
        {
            Android.Manifest.Permission.SendSms
        };

        private const int RequestId = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            //Platform.Init(this, savedInstanceState);
            //RequestPermissions(Permissions, RequestId);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            var alarmIntent = new Intent(this, typeof(AlarmReceiver));

            var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            //alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 25 * 1000, pending);
            alarmManager.SetRepeating(AlarmType.ElapsedRealtimeWakeup, 
                SystemClock.ElapsedRealtime() + 30 * 1000, 
                SystemClock.ElapsedRealtime() + 30 * 1000, pending);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            //Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}