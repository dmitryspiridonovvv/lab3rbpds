namespace FitnessCenterLab3.Models

{
    public class Employee
    {
        public int EmployeeID { get; set; }

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Position { get; set; } = default!;
        public DateTime HireDate { get; set; } = DateTime.Now;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
