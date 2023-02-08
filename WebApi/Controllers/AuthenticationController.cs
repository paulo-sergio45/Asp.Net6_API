using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Model;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SqliteDbContext _context;
        public AuthenticationController(IConfiguration Configuration, SqliteDbContext context)
        {
            _context = context;
            _configuration = Configuration;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponse>> PostUser([FromBody] UserRegister user)
        {
            var userByEmail = await _context.Users.Where(e => e.Email == user.Email).FirstOrDefaultAsync();
            var userByNickName = await _context.Users.Where(e => e.NickName == user.NickName).FirstOrDefaultAsync();
            if (userByEmail is not null || userByNickName is not null)
            {
                return BadRequest("User already exists.");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserResponse(user.Id, user.Name, user.NickName, user.Email));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetUser([FromRoute] long id)
        {
            UserRegister user = await _context.Users.FindAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            return new UserResponse(user.Id, user.Name, user.NickName, user.Email);
        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutUser(long id, [FromBody] UserRegister user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var todoItem = await _context.Users.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Users.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> UserLogin([FromBody] UserLogin model)
        {

            UserRegister user = await _context.Users.Where(e => e.Email.ToLower() == model.Email.ToLower() && e.Password.ToLower() == model.Password.ToLower()).FirstOrDefaultAsync();

            if (user is null)
            {
                return Unauthorized();
            }

            var token = TokenService.GenerateToken(_configuration, user);

            return new
            {
                user = new UserResponse(user.Id, user.Name, user.NickName, user.Email),
                token = token
            };

        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}