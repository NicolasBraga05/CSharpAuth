using CSharpAuth.Dtos;
using CSharpAuth.Database;
using CSharpAuth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace CSharpAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {

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


            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("minha_chave_super_secreta_com_32bytes!!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "sua-api",
                audience: "sua-api-client",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);


            var response = new
            {
                message = "Usuário criado com sucesso!",
                user = userDto,
                token = tokenString
            };

            return Ok(response);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _appDbContext.Project
         .Select(u => new UserDto
         {
             Id = u.Id,
             Nome = u.Name,
             Email = u.Email
         })
         .ToListAsync();

            var response = new UserResponse
            {
                Message = "Lista de usuários recuperada com sucesso!",
                Users = users
            };

            return Ok(response);
        }
    }
}