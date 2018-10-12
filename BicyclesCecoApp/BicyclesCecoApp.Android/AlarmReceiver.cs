using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using BicyclesCecoApp.Droid;
using BicyclesCecoApp.Models;
using Realms;
using System;
using System.Linq;
using System.Net;
using Xamarin.Forms;

namespace BicyclesCecoApp
{
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
       
        public override void OnReceive(Context context, Intent intent)
        {

            Realm _getRealmInstance = Realm.GetInstance();
            var listOfEmployees = _getRealmInstance.All<Employee>().ToList();
            foreach (Employee item in listOfEmployees)
            {
                if (EmployeesManager.ShouldLockDaily(item) ||
                    EmployeesManager.ShouldUnlockDaily(item) ||
                    EmployeesManager.ShouldLockNightly(item) ||
                    EmployeesManager.ShouldUnlockNightly(item)||
                    item.ForceSend)
                {
                    var client = new RestSharp.RestClient("https://gatewayapi.com/rest");
                    var apiKey = "OCC5QlEiXCdMwZ_8QfI4KNCq";
                    var apiSecret = "XBgv72Em&.XtbJE(1(wjoL&6dlA#KYY-Zqs8kDzr";

                    client.Authenticator = RestSharp.Authenticators
                        .OAuth1Authenticator.ForRequestToken(apiKey, apiSecret);
                    var request = new RestSharp.RestRequest("mtsms", RestSharp.Method.POST);
                    request.AddJsonBody(new
                    {
                        sender = "BicyclesCeco",
                        recipients = new[] { new { msisdn = item.CardNumber } },
                        message = item.LockUnlockMessage
                    });
                    var response = client.Execute(request);

                    // On 200 OK, parse the list of SMS IDs else print error
                    if ((int)response.StatusCode == 200)
                    {
                        item.IsLocked = !item.IsLocked;
                        item.ForceSend = false;
                        var res = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                        foreach (var i in res["ids"])
                        {
                            Console.WriteLine(i);
                        }
                    }
                    else if (response.ResponseStatus == RestSharp.ResponseStatus.Completed)
                    {
                        Console.WriteLine(response.Content);
                    }
                    else
                    {
                        Console.WriteLine(response.ErrorMessage);
                    }
                }
            }           

        }
        
    }
}
