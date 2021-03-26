using System;

namespace CRM_System_For_Fitness_Club
{
    class MembershipPurchase
    {
        public Guid MembershipPurchaseID { get; set; }
        public string PurchaseDate { get; set; }
        public decimal PurchaseAmount { get; set; }
    }
}
