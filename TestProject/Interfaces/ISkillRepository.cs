using TestProject.Dto;
using TestProject.Models;

namespace TestProject.Interfaces
{
    public interface ISkillRepository
    {
        ICollection<Skill> GetAll();
        Skill GetById(int id);
        Skill GetByName(string name);
        Skill GetSkillTrimToUpper(SkillDto skillCreate);
        bool CreateSkill(Skill skill);
        bool UpdateSkill(Skill skill);
        bool DeleteSkill(Skill skill);
        bool Save();
        bool IsExist(int id);
        bool AddEmployeeToSkill(int employeeId, int skillId);
        bool RemoveEmployeeFromSkill(int employeeId, int skillId);
    }
}
