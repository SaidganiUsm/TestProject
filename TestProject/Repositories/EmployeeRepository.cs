using AutoMapper;
using TestProject.Data;
using TestProject.Dto;
using TestProject.Interfaces;
using TestProject.Models;

namespace TestProject.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly OfficeDbContext _dataContext;
        private readonly IMapper _mapper;
        public EmployeeRepository(OfficeDbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public bool CreateEmployee(Employee employee)
        {
            _dataContext.Add(employee);
            return Save();
        }

        public bool CreateEmployee(int DepartmentId, Employee employee)
        {
            var EmployeeDepartmentEntity = _dataContext.Employees.Where(a => a.EmployeeId == DepartmentId).FirstOrDefault();
            _dataContext.Add(employee);
            return Save();
        }

        public bool DeleteEmployee(Employee employee)
        {
            _dataContext.Remove(employee);
            return Save();
        }

        public ICollection<Employee> GetAll()
        {
            return _dataContext.Employees.ToList();
        }

        public Employee GetById(int id)
        {
            return _dataContext.Employees.Where(b => b.EmployeeId == id).FirstOrDefault();
        }

        public Employee GetByName(string name)
        {
            return _dataContext.Employees.Where(b => b.Name == name).FirstOrDefault();
        }

        public Employee GetEmployeeTrimToUpper(EmployeeDto EmployeeCreate)
        {
            return GetAll().Where(c => c.Name.Trim().ToUpper() == EmployeeCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
        }

        public bool IsExist(int id)
        {
            return _dataContext.Employees.Where(b => b.EmployeeId == id).Any();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateEmployee(int DepartmentId, Employee employee)
        {
            _dataContext.Update(employee);
            return Save();
        }
    }
}
