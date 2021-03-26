using System;

namespace CRM_System_For_Fitness_Club
{
    class EmployeeSchedule
    {
        public Guid CoachingID { get; set; }
        public Guid CoachID { get; set; }
        public string CoachFullName { get; set; }
        public string CoachingDate { get; set; }
        public string CoachingStart { get; set; }
        public string CoachingEnd { get; set; }
    }
}
