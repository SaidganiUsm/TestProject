namespace TestProject.Models
{
    public class Department
    {
        // Base instances for this object class
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }


        // Reletaionship model "One-to-many"
        public ICollection<Employee> Employees { get; set; }
    }
}
