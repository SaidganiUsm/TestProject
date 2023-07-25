using AutoMapper;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
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
        private readonly JsonSerializerOptions _jsonOptions;
        public EmployeeController(IEmployeeRepository EmployeeRepository, IDepartmentRepository DepartmentRepository, IMapper mapper)
        {
            _EmployeeRepository = EmployeeRepository;
            _DepartmentRepository = DepartmentRepository;
            _mapper = mapper;


            // For returning correct formated json file
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true // Optional: To make the JSON output more readable
            };
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

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult GetEmployeeById(int id)
        {
            // Get the employee by ID from the repository
            var employee = _EmployeeRepository.GetEmployeeById(id);

            if (!_EmployeeRepository.IsExist(id))
                return NotFound();

            // Include projects and skills in the response
            var projects = employee.Projects;
            var skills = employee.Skills;

            // Return the employee with the associated projects and skills
            return Content(JsonSerializer.Serialize(employee, _jsonOptions), "application/json");
        }
    }
}
