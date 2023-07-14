using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestProject.Dto;
using TestProject.Interfaces;
using TestProject.Models;

namespace TestProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _DepartmentRepo;
        private readonly IMapper _mapper;
        public DepartmentController(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _DepartmentRepo = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Department>))]
        public IActionResult GetAll()
        {
            var Departments = _mapper.Map<List<DepartmentDto>>(_DepartmentRepo.GetAll());
            if (ModelState.IsValid)
            {
                return Ok(Departments);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateDepartment([FromQuery] int DepartmentId, [FromBody] DepartmentDto DepartmentCreate)
        {
            if (DepartmentCreate == null)
                return BadRequest(ModelState);

            var Departments = _DepartmentRepo.GetDepartmentTrimToUpper(DepartmentCreate);

            if (Departments != null)
            {
                ModelState.AddModelError("", "Already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var DepartmentMap = _mapper.Map<Department>(DepartmentCreate);


            if (!_DepartmentRepo.CreateDepartment(DepartmentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{DepartmentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateDepartment(int DepartmentId,
            [FromBody] DepartmentDto updateDepartment)
        {
            if (updateDepartment == null)
                return BadRequest(ModelState);

            if (DepartmentId != updateDepartment.DepartmentId)
                return BadRequest(ModelState);

            if (!_DepartmentRepo.IsExist(DepartmentId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var DepartmentMap = _mapper.Map<Department>(updateDepartment);

            if (!_DepartmentRepo.UpdateDepartment(DepartmentMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{DepartmentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDepartment(int DepartmentId)
        {
            if (!_DepartmentRepo.IsExist(DepartmentId))
            {
                return NotFound();
            }
            var DepartmentToDelete = _DepartmentRepo.GetById(DepartmentId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_DepartmentRepo.DeleteDepartment(DepartmentToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer");
            }

            return NoContent();
        }
    }
}
