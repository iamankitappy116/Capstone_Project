using ASPCoreWebAPICRUD.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASPCoreWebAPICRUD.Models;
using Microsoft.EntityFrameworkCore;
namespace ASPCoreWebAPICRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly MyDbContext context;

        public StudentAPIController(MyDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            var data = await context.Students.ToListAsync();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> AddStudent(Student s)
        {
            await context.Students.AddAsync(s);
            await context.SaveChangesAsync();
            return Ok(s);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(int id, Student s)
        {
            if(id != s.Id)
            {
                return BadRequest();
            }

            context.Entry(s).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(s);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            var s = await context.Students.FindAsync(id);

            if (s == null)
            {
                return NotFound();
            }
            context.Students.Remove(s);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
