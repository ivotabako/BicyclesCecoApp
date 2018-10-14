using System;
using System.Collections.Generic;
using System.Text;
using Realms;

namespace BicyclesCecoApp.Models
{
    public class Employee : RealmObject
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        

        public string LockerCode { get; set; }
        public string Deposit { get; set; }
        public string Shift { get; set; }
        public string BicycleId { get; set; }

        public string CardNumber { get; set; }
        public string LockUnlockMessage { get; set; }

        //public bool ReceivedMorning { get; set; }
        //public bool ReceivedEvening { get; set; }

        public bool? HasReceivedConfirmation { get; set; }
        public string MessageId { get; set; }

        public bool IsLocked { get; set; }
        public bool IsInUse { get; set; }

        public bool Manual { get; set; }

        public string PaymentThisWeek { get; set; }
        public string PaymentLastWeek { get; set; }
        public string PaymentTwoWeeksAgo { get; set; }
        public string PaymentThreeWeeksAgo { get; set; }
    }
}
