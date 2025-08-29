using CSharpAuth.Dtos;
using CSharpAuth.Database;
using CSharpAuth.Models;
using Microsoft.AspNetCore.Mvc;


namespace CSharpAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly AppDbContext _appDbContext;

        public UserController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            var existUser = await _appDbContext.Project.FindAsync(user.Id);

            if (existUser != null)
            {
                return Conflict(new { message = "Usuário já existe." });
            }

            _appDbContext.Project.Add(user);
            await _appDbContext.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Nome = user.Name,
                Email = user.Email
            };

            var response = new UserResponse
            {
                Message = "Usuário criado com sucesso!",
                User = userDto
            };

            return Ok(response);
        }
    }
}