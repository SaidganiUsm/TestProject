namespace TestProject.Models
{
    public class Skill
    {
        // Base instances for this object class
        public int SkillId { get; set; }
        public string SkillName { get; set; }


        // Reletaionship model "Many-to-many"
        public ICollection<Employee> Employees { get; set; }
    }
}
