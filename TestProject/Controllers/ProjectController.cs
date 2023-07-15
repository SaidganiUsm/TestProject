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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _ProjectRepository;
        private readonly IMapper _mapper;
        public ProjectController(IProjectRepository projectRepository, IMapper mapper)
        {
            _ProjectRepository = projectRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Project>))]
        public IActionResult GetAll()
        {
            var Projects = _mapper.Map<List<ProjectDto>>(_ProjectRepository.GetAll());
            if (ModelState.IsValid)
            {
                return Ok(Projects);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProject([FromQuery] int ProjectId, [FromBody] ProjectDto ProjectCreate)
        {
            if (ProjectCreate == null)
                return BadRequest(ModelState);

            var Projects = _ProjectRepository.GetProjectTrimToUpper(ProjectCreate);

            if (Projects != null)
            {
                ModelState.AddModelError("", "Already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var ProjectMap = _mapper.Map<Project>(ProjectCreate);


            if (!_ProjectRepository.CreateProject(ProjectMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{ProjectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProject(int ProjectId,
            [FromBody] ProjectDto updateProject)
        {
            if (updateProject == null)
                return BadRequest(ModelState);

            if (ProjectId != updateProject.ProjectId)
                return BadRequest(ModelState);

            if (!_ProjectRepository.IsExist(ProjectId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var ProjectMap = _mapper.Map<Project>(updateProject);

            if (!_ProjectRepository.UpdateProject(ProjectMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ProjectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProject(int ProjectId)
        {
            if (!_ProjectRepository.IsExist(ProjectId))
            {
                return NotFound();
            }
            var ProjectToDelete = _ProjectRepository.GetById(ProjectId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_ProjectRepository.DeleteProject(ProjectToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer");
            }

            return NoContent();
        }


        [HttpGet("employees/{employeeId}/projects")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult GetProjectsByEmployeeId(int employeeId)
        {
            var projects = _ProjectRepository.GetProjectsByEmployeeId(employeeId);
            if (projects == null)
                return NotFound();

            return Ok(projects);
        }

        [HttpGet("{projectId}/employees")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult GetEmployeesByProjectId(int projectId)
        {
            var employees = _ProjectRepository.GetEmployeesByProjectId(projectId);
            if (employees == null)
                return NotFound();

            return Ok(employees);
        }

        [HttpPost("employees/{employeeId}/projects/{projectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult AddEmployeeToProject(int employeeId, int projectId)
        {
            var success = _ProjectRepository.AddEmployeeToProject(employeeId, projectId);
            if (!success)
                return NotFound();

            return Ok();
        }

        [HttpDelete("employees/{employeeId}/projects/{projectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult RemoveEmployeeFromProject(int employeeId, int projectId)
        {
            var success = _ProjectRepository.RemoveEmployeeFromProject(employeeId, projectId);
            if (!success)
                return NotFound();

            return Ok();
        }
    }
}
