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
    [Authorize(Roles = "administrador,inspetor")]
    public class PendenciasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PendenciasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PendenciaDto>>> GetPendencias()
        {
            var pendencias = await _context.Pendencias
                .Include(p => p.Obra)
                    .ThenInclude(o => o.Cliente)
                .OrderByDescending(p => p.Id)
                .Select(p => new PendenciaDto
                {
                    Id = p.Id,
                    PontoIdentificado = p.PontoIdentificado,
                    DescricaoOcorrencia = p.DescricaoOcorrencia,
                    AcaoCorretiva = p.AcaoCorretiva,
                    FotosUrls = p.FotosUrls,
                    Status = p.Status,
                    GrauSeveridade = p.GrauSeveridade,
                    ObraId = p.ObraId,
                    Obra = p.Obra
                })
                .ToListAsync();

            return Ok(pendencias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PendenciaDto>> GetPendencia(int id)
        {
            var p = await _context.Pendencias
                .Include(x => x.Obra)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return NotFound(new { Mensagem = "Pendência não encontrada." });

            return Ok(new PendenciaDto
            {
                Id = p.Id,
                PontoIdentificado = p.PontoIdentificado,
                DescricaoOcorrencia = p.DescricaoOcorrencia,
                AcaoCorretiva = p.AcaoCorretiva,
                FotosUrls = p.FotosUrls,
                Status = p.Status,
                GrauSeveridade = p.GrauSeveridade,
                ObraId = p.ObraId,
                Obra = p.Obra
            });
        }

        [HttpPost]
        public async Task<ActionResult<Pendencia>> CreatePendencia([FromBody] CreatePendenciaDto dto)
        {
            var obraExiste = await _context.Obras.AnyAsync(o => o.Id == dto.ObraId);
            if (!obraExiste) return BadRequest(new { Mensagem = "Obra informada não encontrada." });

            // Valida a regra da tela: 2 fotos obrigatórias separadas por ";"[cite: 11]
            var fotos = dto.FotosUrls?.Split(';', StringSplitOptions.RemoveEmptyEntries);
            if (fotos == null || fotos.Length < 2)
            {
                return BadRequest(new { Mensagem = "É obrigatório anexar 2 evidências fotográficas." });
            }

            var pendencia = new Pendencia
            {
                PontoIdentificado = dto.PontoIdentificado,
                DescricaoOcorrencia = dto.DescricaoOcorrencia,
                AcaoCorretiva = dto.AcaoCorretiva,
                FotosUrls = dto.FotosUrls,
                Status = dto.Status,
                GrauSeveridade = dto.GrauSeveridade,
                ObraId = dto.ObraId
            };

            _context.Pendencias.Add(pendencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPendencia), new { id = pendencia.Id }, pendencia);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] string novoStatus)
        {
            var pendencia = await _context.Pendencias.FindAsync(id);
            if (pendencia == null) return NotFound(new { Mensagem = "Pendência não encontrada." });

            pendencia.Status = novoStatus; // "ABERTA", "EM_ANALISE", "RESOLVIDA"
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> DeletePendencia(int id)
        {
            var pendencia = await _context.Pendencias.FindAsync(id);
            if (pendencia == null) return NotFound(new { Mensagem = "Pendência não encontrada." });

            _context.Pendencias.Remove(pendencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}