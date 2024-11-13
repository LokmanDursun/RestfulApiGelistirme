using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestfulApiGelistirme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RepositoryDbContext _repositoryDbContext;
        public UserController(RepositoryDbContext repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _repositoryDbContext.Users.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _repositoryDbContext.Users.Include(y => y.Tasks).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }


        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            if (user != null)
            {
                _repositoryDbContext.Users.Add(user);
                await _repositoryDbContext.SaveChangesAsync();
            }
            else { return BadRequest(); }
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {

            if (user != null)
            {
                if (id != user.Id)
                    BadRequest();
            }
            _repositoryDbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _repositoryDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_repositoryDbContext.Users.Any(u => u.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _repositoryDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();
            _repositoryDbContext.Remove(user);
            await _repositoryDbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("assign-task/{userId}")]
        public async Task<ActionResult<Entities.Task>> AssignTask(int userId, Entities.Task task)
        {
            var user = await _repositoryDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            task.UserId = userId;
            _repositoryDbContext.Tasks.Add(task);
            await _repositoryDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = userId }, task);
        }

    }
}
