using Realms;

namespace BicyclesCecoApp.Models
{
    //  RealmObject means, Implicit reference conversion from model to realmobject
    public class CustomerDetails : RealmObject
    {
        [PrimaryKey]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerAge { get; set; }
    }
}
