using BicyclesCecoApp.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BicyclesCecoApp
{
    public class SmsManager
    {
        public void Send(Employee item)
        {
            var client = new RestSharp.RestClient("https://gatewayapi.com/rest");
            var apiKey = "OCC5QlEiXCdMwZ_8QfI4KNCq";
            var apiSecret = "XBgv72Em&.XtbJE(1(wjoL&6dlA#KYY-Zqs8kDzr";

            client.Authenticator = RestSharp.Authenticators
                .OAuth1Authenticator.ForRequestToken(apiKey, apiSecret);

            var request = new RestSharp.RestRequest("mtsms", RestSharp.Method.POST);
            request.AddJsonBody(new
            {
                sender = "BiciCeco",
                recipients = new[] { new { msisdn = item.CardNumber } },
                message = item.LockUnlockMessage
            });

            var response = client.Execute(request);

            // On 200 OK, parse the list of SMS IDs else print error
            if ((int)response.StatusCode == 200)
            {
                item.HasReceivedConfirmation = false;
                var res = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                item.MessageId = res["ids"].ToString();
                Console.WriteLine(res);

                //handler res;
                //foreach (var i in res["ids"])
                //{
                //    Console.WriteLine(i);
                //}
            }
            else if (response.ResponseStatus == RestSharp.ResponseStatus.Completed)
            {
                Console.WriteLine(response.Content);
                // response.Content;
            }
            else
            {
                Console.WriteLine(response.ErrorMessage);
                //return response.ErrorMessage;
            }
        }

        public void GetConfirmation(Employee item)
        {
            if (item.HasReceivedConfirmation.HasValue && item.HasReceivedConfirmation.Value)
            {
                return;
            }

            var client = new RestSharp.RestClient("https://gatewayapi.com/rest");
            var apiKey = "OCC5QlEiXCdMwZ_8QfI4KNCq";
            var apiSecret = "XBgv72Em&.XtbJE(1(wjoL&6dlA#KYY-Zqs8kDzr";

            client.Authenticator = RestSharp.Authenticators
                .OAuth1Authenticator.ForRequestToken(apiKey, apiSecret);

            var request = new RestSharp.RestRequest("mtsms/" + item.MessageId, RestSharp.Method.GET);
            

            var response = client.Execute(request);

            // On 200 OK, parse the list of SMS IDs else print error
            if ((int)response.StatusCode == 200)
            {                
                var res = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                if(res["dsnstatus"].ToString() == "DELIVERED")
                {
                    item.HasReceivedConfirmation = true;
                    item.IsLocked = !item.IsLocked;
                }
                
                Console.WriteLine(res);                                
            }
            else if (response.ResponseStatus == RestSharp.ResponseStatus.Completed)
            {
                Console.WriteLine(response.Content);
                // response.Content;
            }
            else
            {
                Console.WriteLine(response.ErrorMessage);
                //return response.ErrorMessage;
            }
        }
    }
}
