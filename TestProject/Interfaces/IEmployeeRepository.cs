using TestProject.Dto;
using TestProject.Models;

namespace TestProject.Interfaces
{
    public interface IEmployeeRepository
    {
        ICollection<Employee> GetAll();
        Employee GetById(int id);
        Employee GetByName(string name);
        Employee GetEmployeeTrimToUpper(EmployeeDto EmployeeCreate);
        bool CreateEmployee(int DepartmentId, Employee Employee);
        bool UpdateEmployee(int DepartmentId, Employee Employee);
        bool DeleteEmployee(Employee Employee);
        bool Save();
        bool IsExist(int id);

        // Many-to-many operations for Project
        ICollection<Project> GetProjectsByEmployeeId(int employeeId);
        ICollection<Employee> GetEmployeesByProjectId(int projectId);
        bool AddEmployeeToProject(int employeeId, int projectId);
        bool RemoveEmployeeFromProject(int employeeId, int projectId);


        // Many-to-many operations for Skill
        ICollection<Skill> GetSkillsByEmployeeId(int employeeId);
        ICollection<Employee> GetEmployeesBySkillId(int skillId);
        bool AddSkillToEmployee(int employeeId, int skillId);
        bool RemoveSkillFromEmployee(int employeeId, int skillId);
    }
}
