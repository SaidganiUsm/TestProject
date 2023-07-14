namespace TestProject.Models
{
    public class Employee
    {
        // Base instances for this object class
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }


        // Reletaionship model "One-to-many"
        public int DepartmentId { get; set; }
        public Department Department { get; set; }


        // Reletaionship model "Many-to-many"
        public ICollection<Project> Projects { get; set; }
        public ICollection<Skill> Skills { get; set; }
    }
}
