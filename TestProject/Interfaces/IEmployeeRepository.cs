using TestProject.Dto;
using TestProject.Models;

namespace TestProject.Interfaces
{
    public interface IEmployeeRepository
    {
        ICollection<Employee> GetAll();
        Employee GetById(int id);
        Employee GetEmployeeById(int id);
        Employee GetByName(string name);
        Employee GetEmployeeTrimToUpper(EmployeeDto employeeCreate);
        bool CreateEmployee(int DepartmentId, Employee employee);
        bool UpdateEmployee(int DepartmentId, Employee employee);
        bool DeleteEmployee(Employee employee);
        bool Save();
        bool IsExist(int id);
    }
}
