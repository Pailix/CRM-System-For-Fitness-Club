//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRM_System_For_Fitness_Club
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientMembership
    {
        public System.Guid ClientMembership_ID { get; set; }
        public Nullable<System.Guid> Client_ID { get; set; }
        public Nullable<int> Membership_ID { get; set; }
        public Nullable<byte> RemainingVisits { get; set; }
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual MembershipClassification MembershipClassification { get; set; }
    }
}