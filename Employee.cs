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
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.CoachingSchedules = new HashSet<CoachingSchedule>();
            this.EmployeesSalaries = new HashSet<EmployeesSalary>();
            this.Users = new HashSet<User>();
        }
    
        public System.Guid Employee_ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Nullable<int> Position_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CoachingSchedule> CoachingSchedules { get; set; }
        public virtual Position Position { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeesSalary> EmployeesSalaries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
