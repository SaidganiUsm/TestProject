using AutoMapper;
using System.Xml.Linq;
using TestProject.Data;
using TestProject.Dto;
using TestProject.Interfaces;
using TestProject.Models;

namespace TestProject.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly OfficeDbContext _dataContext;
        private readonly IMapper _mapper;
        public DepartmentRepository(OfficeDbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public bool CreateDepartment(Department department)
        {
            _dataContext.Add(department);
            return Save();
        }

        public bool DeleteDepartment(Department department)
        {
            _dataContext.Remove(department);
            return Save();
        }

        public ICollection<Department> GetAll()
        {
            return _dataContext.Department.ToList();
        }

        public Department GetById(int id)
        {
            return _dataContext.Department.Where(b => b.DepartmentId == id).FirstOrDefault();
        }

        public Department GetByName(string name)
        {
            return _dataContext.Department.Where(b => b.DepartmentName == name).FirstOrDefault();
        }

        public Department GetDepartmentTrimToUpper(DepartmentDto departmentCreate)
        {
            return GetAll().Where(c => c.DepartmentName.Trim().ToUpper() == departmentCreate.DepartmentName.TrimEnd().ToUpper())
                .FirstOrDefault();
        }

        public bool IsExist(int id)
        {
            return _dataContext.Department.Where(b => b.DepartmentId == id).Any();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateDepartment(Department department)
        {
            _dataContext.Update(department);
            return Save();
        }
    }
}
