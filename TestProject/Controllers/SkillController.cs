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
    public class SkillController : ControllerBase
    {
        private readonly ISkillRepository _SkillRepository;
        private readonly IMapper _mapper;
        public SkillController(ISkillRepository SkillRepository, IMapper mapper)
        {
            _SkillRepository = SkillRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Skill>))]
        public IActionResult GetAll()
        {
            var Skills = _mapper.Map<List<SkillDto>>(_SkillRepository.GetAll());
            if (ModelState.IsValid)
            {
                return Ok(Skills);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSkill([FromQuery] int SkillId, [FromBody] SkillDto SkillCreate)
        {
            if (SkillCreate == null)
                return BadRequest(ModelState);

            var Skills = _SkillRepository.GetSkillTrimToUpper(SkillCreate);

            if (Skills != null)
            {
                ModelState.AddModelError("", "Already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var SkillMap = _mapper.Map<Skill>(SkillCreate);


            if (!_SkillRepository.CreateSkill(SkillMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{SkillId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateSkill(int SkillId,
            [FromBody] SkillDto updateSkill)
        {
            if (updateSkill == null)
                return BadRequest(ModelState);

            if (SkillId != updateSkill.SkillId)
                return BadRequest(ModelState);

            if (!_SkillRepository.IsExist(SkillId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var SkillMap = _mapper.Map<Skill>(updateSkill);

            if (!_SkillRepository.UpdateSkill(SkillMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{SkillId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSkill(int SkillId)
        {
            if (!_SkillRepository.IsExist(SkillId))
            {
                return NotFound();
            }
            var SkillToDelete = _SkillRepository.GetById(SkillId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_SkillRepository.DeleteSkill(SkillToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer");
            }

            return NoContent();
        }

        [HttpPost("employees/{employeeId}/Skill/{SkillId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult AddEmployeeToSkill(int employeeId, int SkillId)
        {
            var success = _SkillRepository.AddEmployeeToSkill(employeeId, SkillId);
            if (!success)
                return NotFound();

            return Ok();
        }

        [HttpDelete("employees/{employeeId}/Skill/{SkillId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult RemoveEmployeeFromSkill(int employeeId, int SkillId)
        {
            var success = _SkillRepository.RemoveEmployeeFromSkill(employeeId, SkillId);
            if (!success)
                return NotFound();

            return Ok();
        }
    }
}
