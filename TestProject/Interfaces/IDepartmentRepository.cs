using TestProject.Dto;
using TestProject.Models;

namespace TestProject.Interfaces
{
    public interface IDepartmentRepository
    {
        ICollection<Department> GetAll();
        Department GetById(int id);
        Department GetByName(string name);
        Department GetDepartmentTrimToUpper(DepartmentDto departmentCreate);
        bool CreateDepartment(Department department);
        bool UpdateDepartment(Department department);
        bool DeleteDepartment(Department department);
        bool Save();
        bool IsExist(int id);
    }
}
