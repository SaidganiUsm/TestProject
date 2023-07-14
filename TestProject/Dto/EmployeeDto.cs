using TestProject.Models;

namespace TestProject.Dto
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }

        public ICollection<Project> Projects { get; set; }
        public ICollection<Skill> Skills { get; set; }
    }
}
