using TestProject.Models;

namespace TestProject.Interfaces
{
    public interface IEmployeeRepository
    {
        ICollection<Employee> GetAll();
        Employee GetById(int id);
        Employee GetByName(string name);
        Employee GetEmployeeTrimToUpper(EmployeeDto EmployeeCreate);
        bool CreateEmployee(int teacherId, Employee Employee);
        bool UpdateEmployee(int teacherId, Employee Employee);
        bool DeleteEmployee(Employee Employee);
        bool Save();
        bool IsExist(int id);
    }
}
