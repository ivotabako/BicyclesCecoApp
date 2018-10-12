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

        
    }
}
