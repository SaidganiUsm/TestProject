using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Dto;
using TestProject.Interfaces;
using TestProject.Models;

namespace TestProject.Repositories
{
    public class ProjectRepository  : IProjectRepository
    {
        private readonly OfficeDbContext _dataContext;
        private readonly IMapper _mapper;
        public ProjectRepository(OfficeDbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public bool CreateProject(Project project)
        {
            _dataContext.Add(project);
            return Save();
        }

        public bool DeleteProject(Project project)
        {
            _dataContext.Remove(project);
            return Save();
        }

        public ICollection<Project> GetAll()
        {
            return _dataContext.Projects.ToList();
        }

        public Project GetById(int id)
        {
            return _dataContext.Projects.Where(b => b.ProjectId == id).FirstOrDefault();
        }

        public Project GetByName(string name)
        {
            return _dataContext.Projects.Where(b => b.ProjectName == name).FirstOrDefault();
        }

        public Project GetProjectTrimToUpper(ProjectDto ProjectCreate)
        {
            return GetAll().Where(c => c.ProjectName.Trim().ToUpper() == ProjectCreate.ProjectName.TrimEnd().ToUpper())
                .FirstOrDefault();
        }

        public bool IsExist(int id)
        {
            return _dataContext.Projects.Where(b => b.ProjectId == id).Any();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProject(Project project)
        {
            _dataContext.Update(project);
            return Save();
        }

        public ICollection<Project> GetProjectsByEmployeeId(int employeeId)
        {
            return _dataContext.Set<Employee>()
                .Include(e => e.Projects)
                .FirstOrDefault(e => e.EmployeeId == employeeId)?.Projects;
        }

        public ICollection<Employee> GetEmployeesByProjectId(int projectId)
        {
            var project = _dataContext.Set<Project>()
            .Include(p => p.Employees)
            .FirstOrDefault(p => p.ProjectId == projectId);

            if (project == null)
                return null; // or return an empty collection

            return project.Employees;
        }

        public bool AddEmployeeToProject(int employeeId, int projectId)
        {
            var employee = _dataContext.Set<Employee>()
            .Include(e => e.Projects)
            .FirstOrDefault(e => e.EmployeeId == employeeId);

            var project = _dataContext.Set<Project>().Find(projectId);

            if (employee == null || project == null)
                return false;

            if (employee.Projects == null)
                employee.Projects = new List<Project>();

            employee.Projects.Add(project);
            _dataContext.SaveChanges();
            return true;
        }

        public bool RemoveEmployeeFromProject(int employeeId, int projectId)
        {
            var employee = _dataContext.Set<Employee>()
            .Include(e => e.Projects)
            .FirstOrDefault(e => e.EmployeeId == employeeId);

            var project = _dataContext.Set<Project>().Find(projectId);

            if (employee == null || project == null)
                return false;

            employee.Projects.Remove(project);
            _dataContext.SaveChanges();
            return true;
        }
    }
}
