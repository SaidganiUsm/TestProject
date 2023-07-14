namespace TestProject.Models
{
    public class Project
    {
        // Base instances for this object class
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }


        // Reletaionship model "Many-to-many"
        public ICollection<Employee> Employees { get; set; }
    }
}
