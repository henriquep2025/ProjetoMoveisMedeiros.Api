using System.Text.Json;
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
    [Authorize(Roles = "administrador,inspetor,montador")]
    public class ObrasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ObrasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetObras()
        {
            // Retorna os campos do ObraDto + dados básicos do Cliente para exibir nos cards
            var obras = await _context.Obras
                .Include(o => o.Cliente)
                .OrderByDescending(o => o.Id)
                .Select(o => new
                {
                    o.Id,
                    o.NomeProjeto,
                    o.Status,
                    o.DataPrevistaInicio,
                    o.DataEstimadaEntrega,
                    o.DataEntregaReal,
                    o.AmbientesContratados,
                    o.MateriaisInsumos,
                    o.Preco,
                    o.Lote,
                    o.AmbienteMovel,
                    o.TipoMovel,
                    o.DescricaoMovel,
                    o.CorAcabamento,
                    o.Dimensoes,
                    o.ObservacoesProjeto,
                    o.ChecklistJson,
                    o.DataValidacaoChecklist,
                    o.ClienteId,
                    ClienteNome = o.Cliente.Nome,
                    ClienteEndereco = o.Cliente.Endereco,
                    ClienteCep = o.Cliente.Cep
                })
                .ToListAsync();

            return Ok(obras);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetObra(int id)
        {
            var o = await _context.Obras
                .Include(x => x.Cliente)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (o == null) return NotFound(new { Mensagem = "Obra não encontrada." });

            return Ok(new
            {
                o.Id,
                o.NomeProjeto,
                o.Status,
                o.DataPrevistaInicio,
                o.DataEstimadaEntrega,
                o.DataEntregaReal,
                o.AmbientesContratados,
                o.MateriaisInsumos,
                o.Preco,
                o.Lote,
                o.AmbienteMovel,
                o.TipoMovel,
                o.DescricaoMovel,
                o.CorAcabamento,
                o.Dimensoes,
                o.ObservacoesProjeto,
                o.ChecklistJson,
                o.DataValidacaoChecklist,
                o.ClienteId,
                ClienteNome = o.Cliente.Nome,
                ClienteEndereco = o.Cliente.Endereco,
                ClienteCep = o.Cliente.Cep
            });
        }

        [HttpPost]
        [Authorize(Roles = "administrador")]
        public async Task<ActionResult<Obra>> CreateObra([FromBody] CriarObraDto dto)
        {
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            if (!clienteExiste) return BadRequest(new { Mensagem = "Cliente informado não existe." });

            // Inicializa automaticamente os 5 itens padrão da tela de Checklist[cite: 4]
            var checklistPadrao = new[]
            {
                new { ordem = 1, titulo = "Chapa e Substrato", descricao = "Inspeção de empenamento, espessura e umidade aparente do MDF.", status = "PENDENTE", observacaoDefeito = "", fotos = new List<string>() },
                new { ordem = 2, titulo = "Cor e Textura", descricao = "Uniformidade tonal sob iluminação da obra e compatibilidade com projeto.", status = "PENDENTE", observacaoDefeito = "", fotos = new List<string>() },
                new { ordem = 3, titulo = "Fita de Borda", descricao = "Aderência, excesso de cola, acabamento de refilamento e cantoneiras.", status = "PENDENTE", observacaoDefeito = "", fotos = new List<string>() },
                new { ordem = 4, titulo = "Junções e Emendas", descricao = "Alinhamento de 45 graus, prumo, nível de montantes e esquadro geral.", status = "PENDENTE", observacaoDefeito = "", fotos = new List<string>() },
                new { ordem = 5, titulo = "Outros", descricao = "Materiais que não foram listados acima.", status = "PENDENTE", observacaoDefeito = "", fotos = new List<string>() }
            };

            var obra = new Obra
            {
                NomeProjeto = dto.NomeProjeto,
                Status = dto.Status,
                DataPrevistaInicio = dto.DataPrevistaInicio,
                DataEstimadaEntrega = dto.DataEstimadaEntrega,
                AmbientesContratados = dto.AmbientesContratados,
                MateriaisInsumos = dto.MateriaisInsumos,
                Preco = dto.Preco,
                Lote = dto.Lote,
                AmbienteMovel = dto.AmbienteMovel,
                TipoMovel = dto.TipoMovel,
                DescricaoMovel = dto.DescricaoMovel,
                CorAcabamento = dto.CorAcabamento,
                Dimensoes = dto.Dimensoes,
                ObservacoesProjeto = dto.ObservacoesProjeto,
                ClienteId = dto.ClienteId,
                ChecklistJson = JsonSerializer.Serialize(checklistPadrao)
            };

            _context.Obras.Add(obra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetObra), new { id = obra.Id }, obra);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> UpdateObra(int id, [FromBody] ObraDto dto)
        {
            var obra = await _context.Obras.FindAsync(id);
            if (obra == null) return NotFound(new { Mensagem = "Obra não encontrada." });

            obra.NomeProjeto = dto.NomeProjeto ?? obra.NomeProjeto;
            obra.Status = dto.Status ?? obra.Status;
            obra.DataPrevistaInicio = dto.DataPrevistaInicio;
            obra.DataEstimadaEntrega = dto.DataEstimadaEntrega;
            obra.DataEntregaReal = dto.DataEntregaReal;
            obra.AmbientesContratados = dto.AmbientesContratados ?? obra.AmbientesContratados;
            obra.MateriaisInsumos = dto.MateriaisInsumos ?? obra.MateriaisInsumos;
            obra.Preco = dto.Preco;
            obra.Lote = dto.Lote;
            obra.AmbienteMovel = dto.AmbienteMovel;
            obra.TipoMovel = dto.TipoMovel;
            obra.DescricaoMovel = dto.DescricaoMovel;
            obra.CorAcabamento = dto.CorAcabamento;
            obra.Dimensoes = dto.Dimensoes;
            obra.ObservacoesProjeto = dto.ObservacoesProjeto;
            obra.ClienteId = dto.ClienteId;

            if (obra.Status == "CONCLUIDA" && obra.DataEntregaReal == null)
            {
                obra.DataEntregaReal = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Endpoint específico para Montador, Inspetor ou Admin salvar o Checklist[cite: 4]
        [HttpPut("{id}/checklist")]
        public async Task<IActionResult> SalvarChecklist(int id, [FromBody] string checklistJson)
        {
            var obra = await _context.Obras.FindAsync(id);
            if (obra == null) return NotFound(new { Mensagem = "Obra não encontrada." });

            obra.ChecklistJson = checklistJson;
            obra.DataValidacaoChecklist = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new { Mensagem = "Checklist salvo com sucesso." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> DeleteObra(int id)
        {
            var obra = await _context.Obras.FindAsync(id);
            if (obra == null) return NotFound(new { Mensagem = "Obra não encontrada." });

            _context.Obras.Remove(obra);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}