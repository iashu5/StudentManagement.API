using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly ILogger<StudentsController> _logger;  // Changed

        public StudentsController(StudentService studentService, ILogger<StudentsController> logger)  // Changed
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetAll()
        {
            _logger.LogInformation("Getting all students");  // Changed to LogInformation
            var students = await _studentService.GetAllStudents();
            return Ok(ApiResponse<object>.Ok(students));
        }

        // Other methods remain the same but change _logger.Information to _logger.LogInformation
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetById(int id)
        {
            _logger.LogInformation($"Getting student with ID: {id}");
            var student = await _studentService.GetStudentById(id);
            if (student == null)
                return NotFound(ApiResponse<object>.Fail($"Student with ID {id} not found"));

            return Ok(ApiResponse<object>.Ok(student));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] CreateStudentDto student)
        {
            _logger.LogInformation($"Creating new student with email: {student.Email}");
            try
            {
                var id = await _studentService.CreateStudent(student);
                var newStudent = await _studentService.GetStudentById(id);
                return Ok(ApiResponse<object>.Ok(newStudent, "Student created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] UpdateStudentDto student)
        {
            _logger.LogInformation($"Updating student with ID: {id}");
            try
            {
                var result = await _studentService.UpdateStudent(id, student);
                if (!result)
                    return NotFound(ApiResponse<object>.Fail($"Student with ID {id} not found"));

                var updatedStudent = await _studentService.GetStudentById(id);
                return Ok(ApiResponse<object>.Ok(updatedStudent, "Student updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            _logger.LogInformation($"Deleting student with ID: {id}");
            var result = await _studentService.DeleteStudent(id);
            if (!result)
                return NotFound(ApiResponse<object>.Fail($"Student with ID {id} not found"));

            return Ok(ApiResponse<object>.Ok(null, "Student deleted successfully"));
        }
    }
}