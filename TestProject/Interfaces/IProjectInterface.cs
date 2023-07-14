using TestProject.Models;

namespace TestProject.Interfaces
{
    public interface IProjectInterface
    {
        ICollection<Project> GetAll();
        Project GetById(int id);
        Project GetByName(string name);
        bool CreateProject(Project project);
        bool UpdateProject(Project project);
        bool DeleteProject(Project project);
        bool Save();
        bool IsExist(int id);
    }
}
