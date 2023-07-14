using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestProject.Dto;
using TestProject.Interfaces;
using TestProject.Models;
using TestProject.Repositories;

namespace TestProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IDepartmentRepository _DepartmentRepository;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeRepository EmployeeRepository, IDepartmentRepository DepartmentRepository, IMapper mapper)
        {
            _EmployeeRepository = EmployeeRepository;
            _DepartmentRepository = DepartmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Employee>))]
        public IActionResult GetAll()
        {
            var Employees = _mapper.Map<List<EmployeeDto>>(_EmployeeRepository.GetAll());
            if (ModelState.IsValid)
            {
                return Ok(Employees);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateEmployee([FromQuery] int DepartmentId, [FromBody] EmployeeDto EmployeeCreate)
        {
            if (EmployeeCreate == null)
                return BadRequest(ModelState);

            var Employees = _EmployeeRepository.GetEmployeeTrimToUpper(EmployeeCreate);

            if (Employees != null)
            {
                ModelState.AddModelError("", "Already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var EmployeeMap = _mapper.Map<Employee>(EmployeeCreate);
            EmployeeMap.Department = _DepartmentRepository.GetById(DepartmentId);
            var EmployeeMap2 = _mapper.Map<EmployeeDto>(EmployeeMap);

            if (!_EmployeeRepository.CreateEmployee(DepartmentId, EmployeeMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{EmployeeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateEmployee(int EmployeeId, [FromQuery] int DepartmentId,
            [FromBody] EmployeeDto updateEmployee)
        {
            if (updateEmployee == null)
                return BadRequest(ModelState);

            if (EmployeeId != updateEmployee.EmployeeId)
                return BadRequest(ModelState);

            if (!_EmployeeRepository.IsExist(EmployeeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var EmployeeMap = _mapper.Map<Employee>(updateEmployee);

            if (!_EmployeeRepository.UpdateEmployee(DepartmentId, EmployeeMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{EmployeeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteEmployee(int EmployeeId)
        {
            if (!_EmployeeRepository.IsExist(EmployeeId))
            {
                return NotFound();
            }
            var EmployeeToDelete = _EmployeeRepository.GetById(EmployeeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_EmployeeRepository.DeleteEmployee(EmployeeToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer");
            }
            return NoContent();
        }
    }
}
