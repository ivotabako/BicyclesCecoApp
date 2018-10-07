//using Plugin.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BicyclesCecoApp
{
    public class SmsManager
    {
        public async Task SendSms(string messageText, string recipient)
        {
            try
            {
                var message = new SmsMessage(messageText, recipient);
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        //public static void SendSampleBackgroundSms(this ISmsTask smsTask)
        //{
        //    if (smsTask.CanSendSmsInBackground)
        //    {
        //        smsTask.SendSmsInBackground("+34687612919", "Well hello there from Xam.Messaging.Plugin. Some special characters. ÇÇẪ");
        //    }
        //}

        //public static void SendSampleMultipleSms(this ISmsTask smsTask)
        //{
        //    if (smsTask.CanSendSms)
        //    {
        //        smsTask.SendSms("+272193332320000;+272323219330001", "Well hello there from Xam.Messaging.Plugin");
        //    }
        //}
    }
}
