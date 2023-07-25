using TestProject.Dto;
using TestProject.Models;

namespace TestProject.Interfaces
{
    public interface IProjectRepository
    {
        ICollection<Project> GetAll();
        Project GetById(int id);
        Project GetByName(string name);
        Project GetProjectTrimToUpper(ProjectDto projectCreate);
        bool CreateProject(Project project);
        bool UpdateProject(Project project);
        bool DeleteProject(Project project);
        bool Save();
        bool IsExist(int id);
        bool AddEmployeeToProject(int employeeId, int projectId);
        bool RemoveEmployeeFromProject(int employeeId, int projectId);
    }
}
