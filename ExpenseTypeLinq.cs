using System;

namespace CRM_System_For_Fitness_Club
{
    class ExpenseTypeLinq
    {
        public Guid ExpenseID { get; set; }
        public string ExpenseDate { get; set; }
        public int TypeID { get; set; }
        public string ExpenseType { get; set; }
        public decimal ExpenseCost { get; set; }
    }
}
