using System;

namespace CRM_System_For_Fitness_Club
{
    class ClientMembershipLinq
    {
        public Guid Client_ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public byte RemainingVisits { get; set; }
    }
}
