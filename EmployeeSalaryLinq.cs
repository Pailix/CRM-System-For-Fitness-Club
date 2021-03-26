using System;

namespace CRM_System_For_Fitness_Club
{
    class EmployeeSalaryLinq
    {
        public Guid SalaryID { get; set; }
        public Guid EmployeeID { get; set; }
        public string FullName { get; set; }
        public string SalaryDate { get; set; }
        public decimal SalaryAmount { get; set; }
    }
}
