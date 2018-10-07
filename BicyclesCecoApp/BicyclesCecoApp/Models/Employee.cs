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
        public string Card { get; set; }
        public string LockerCode { get; set; }
        public string Deposit { get; set; }
        public string Shift { get; set; }
        public string BicycleId { get; set; }

        public string PaymentThisWeek { get; set; }
        public string PaymentLastWeek { get; set; }
        public string PaymentTwoWeeksAgo { get; set; }
        public string PaymentThreeWeeksAgo { get; set; }
    }
}
