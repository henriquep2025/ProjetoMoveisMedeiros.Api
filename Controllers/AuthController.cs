using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoMoveisMedeiros.Api.Data;
using ProjetoMoveisMedeiros.Api.Models;

namespace ProjetoMoveisMedeiros.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public AuthController(
            AppDbContext context,
            IConfiguration configuration,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        // Apenas administrador cadastra novos usuários
        [HttpPost("register")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Register([FromBody] RegistroDto registroDto)
        {
            var roleNormalizada = registroDto.Role?.Trim().ToLower();
            var rolesPermitidas = new[] { "administrador", "inspetor", "montador" };

            if (string.IsNullOrEmpty(roleNormalizada) || !rolesPermitidas.Contains(roleNormalizada))
            {
                return BadRequest(new { Mensagem = "Cargo inválido. Valores aceitos: administrador, inspetor ou montador." });
            }

            var emailNormalizado = registroDto.Email.Trim().ToLower();

            if (await _context.Usuarios.AnyAsync(u => u.Email == emailNormalizado))
            {
                return BadRequest(new { Mensagem = "Este e-mail já está cadastrado." });
            }

            var novoUsuario = new Usuario
            {
                Email = emailNormalizado,
                Role = roleNormalizada
            };

            novoUsuario.PasswordHash = _passwordHasher.HashPassword(novoUsuario, registroDto.Password);

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return Ok(new { Mensagem = "Usuário cadastrado com sucesso!" });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var emailNormalizado = loginDto.Email.Trim().ToLower();
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailNormalizado);

            if (user == null)
            {
                return Unauthorized(new { Mensagem = "E-mail ou senha inválidos." });
            }

            var resultadoSenha = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

            if (resultadoSenha == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { Mensagem = "E-mail ou senha inválidos." });
            }

            var token = GerarToken(user);

            return Ok(new
            {
                token,
                id = user.Id,
                email = user.Email,
                role = user.Role
            });
        }

        private string GerarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Chave JWT não configurada.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}