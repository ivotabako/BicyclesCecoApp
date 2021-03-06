﻿using Android.App;
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
                if ((EmployeesManager.ShouldLockDaily(item) ||
                    EmployeesManager.ShouldUnlockDaily(item) ||
                    EmployeesManager.ShouldLockNightly(item) ||
                    EmployeesManager.ShouldUnlockNightly(item)) &&
                    !item.Manual &&
                    (!item.HasReceivedConfirmation.HasValue ||
                        (item.HasReceivedConfirmation.HasValue && item.HasReceivedConfirmation.Value)
                    )
                )
                {
                    _getRealmInstance.Write(() =>
                    {
                        var mng = new SmsManager();
                        mng.Send(item);
                        _getRealmInstance.Add(item, update: true);
                    });
                }

                if (item.HasReceivedConfirmation.HasValue && !item.HasReceivedConfirmation.Value)
                {
                    _getRealmInstance.Write(() =>
                    {
                        var mng = new SmsManager();
                        mng.GetConfirmation(item);
                        _getRealmInstance.Add(item, update: true);
                    });
                }
            }
        }
    }
}
