using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMoveisMedeiros.Api.Data;
using ProjetoMoveisMedeiros.Api.Models;
using ProjetoMoveisMedeiros.Api.Models.DTOs;

namespace ProjetoMoveisMedeiros.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "administrador")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    TipoPessoa = c.TipoPessoa,
                    Nome = c.Nome,
                    Documento = c.Documento,
                    DataNascimento = c.DataNascimento,
                    Email = c.Email,
                    Telefone = c.Telefone,
                    Cep = c.Cep,
                    Endereco = c.Endereco
                })
                .ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetCliente(int id)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null) return NotFound(new { Mensagem = "Cliente não encontrado." });

            return Ok(new ClienteDto
            {
                Id = c.Id,
                TipoPessoa = c.TipoPessoa,
                Nome = c.Nome,
                Documento = c.Documento,
                DataNascimento = c.DataNascimento,
                Email = c.Email,
                Telefone = c.Telefone,
                Cep = c.Cep,
                Endereco = c.Endereco
            });
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDto>> CreateCliente([FromBody] CreateClienteDto dto)
        {
            var cliente = new Cliente
            {
                TipoPessoa = dto.TipoPessoa,
                Nome = dto.Nome,
                Documento = dto.Documento,
                DataNascimento = dto.DataNascimento,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Cep = dto.Cep,
                Endereco = dto.Endereco
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] CreateClienteDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound(new { Mensagem = "Cliente não encontrado." });

            cliente.TipoPessoa = dto.TipoPessoa;
            cliente.Nome = dto.Nome;
            cliente.Documento = dto.Documento;
            cliente.DataNascimento = dto.DataNascimento;
            cliente.Email = dto.Email;
            cliente.Telefone = dto.Telefone;
            cliente.Cep = dto.Cep;
            cliente.Endereco = dto.Endereco;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound(new { Mensagem = "Cliente não encontrado." });

            var possuiObras = await _context.Obras.AnyAsync(o => o.ClienteId == id);
            if (possuiObras)
            {
                return BadRequest(new { Mensagem = "Não é possível excluir um cliente que possui obras vinculadas." });
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}