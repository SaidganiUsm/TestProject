using TestProject.Models;

namespace TestProject.Dto
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
