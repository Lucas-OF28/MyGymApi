using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGymApi.Api.Data;
using MyGymApi.Api.Models;
using MyGymApi.Api.DTOs;
using AutoMapper;

namespace MyGymApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
        private readonly AcademiaContext _context;
        private readonly IMapper _mapper;

        public AlunoController(AcademiaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alunos = await _context.Alunos.ToListAsync();
            var alunosDto = _mapper.Map<List<AlunoDto>>(alunos);
            return Ok(alunosDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return NotFound();
            return Ok(_mapper.Map<AlunoDto>(aluno));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAlunoDto dto)
        {
            var aluno = _mapper.Map<Aluno>(dto);
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();

            var alunoDto = _mapper.Map<AlunoDto>(aluno);
            return CreatedAtAction(nameof(GetById), new { id = aluno.Id }, alunoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAlunoDto dto)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return NotFound();

            _mapper.Map(dto, aluno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return NotFound();

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/relatorio")]
        public async Task<IActionResult> GetRelatorio(int id)
        {
            var aluno = await _context.Alunos
                .Include(a => a.Agendamentos)
                .ThenInclude(ag => ag.Aula)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aluno == null)
                return NotFound("Aluno não encontrado.");

            var hoje = DateTime.Now;
            var mes = hoje.Month;
            var ano = hoje.Year;

            var agendamentosDoMes = aluno.Agendamentos
                .Where(a => a.Aula.DataHora.Month == mes && a.Aula.DataHora.Year == ano)
                .ToList();

            var totalNoMes = agendamentosDoMes.Count;

            var tiposMaisFrequentes = agendamentosDoMes
                .GroupBy(a => a.Aula.Tipo)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    Tipo = g.Key,
                    Quantidade = g.Count()
                })
                .ToList();

            return Ok(new
            {
                Aluno = aluno.Nome,
                Plano = aluno.Plano.ToString(),
                TotalDeAulasNoMes = totalNoMes,
                TiposMaisFrequentes = tiposMaisFrequentes
            });
        }
    }
}
