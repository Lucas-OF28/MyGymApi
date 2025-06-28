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
    public class AgendamentoController : ControllerBase
    {
        private readonly AcademiaContext _context;
        private readonly IMapper _mapper;

        public AgendamentoController(AcademiaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Agendar([FromQuery] int alunoId, [FromQuery] int aulaId)
        {
            var aluno = await _context.Alunos
                .Include(a => a.Agendamentos)
                .ThenInclude(ag => ag.Aula)
                .FirstOrDefaultAsync(a => a.Id == alunoId);

            if (aluno == null)
                return NotFound("Aluno não encontrado.");

            var aula = await _context.Aulas
                .Include(a => a.Agendamentos)
                .FirstOrDefaultAsync(a => a.Id == aulaId);

            if (aula == null)
                return NotFound("Aula não encontrada.");

            if (await _context.Agendamentos.AnyAsync(a => a.AlunoId == alunoId && a.AulaId == aulaId))
                return BadRequest("Aluno já está agendado nessa aula.");

            if (aula.Agendamentos.Count >= aula.CapacidadeMaxima)
                return BadRequest("Capacidade máxima da aula atingida.");

            var mes = aula.DataHora.Month;
            var ano = aula.DataHora.Year;

            int totalAgendadoNoMes = aluno.Agendamentos
                .Count(a => a.Aula.DataHora.Month == mes && a.Aula.DataHora.Year == ano);

            int limitePlano = aluno.Plano switch
            {
                Plano.Mensal => 12,
                Plano.Trimestral => 20,
                Plano.Anual => 30,
                _ => 0
            };

            if (totalAgendadoNoMes >= limitePlano)
                return BadRequest($"Aluno atingiu o limite de {limitePlano} aulas no plano.");

            var agendamento = new Agendamento
            {
                AlunoId = alunoId,
                AulaId = aulaId
            };

            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensagem = "Agendamento realizado com sucesso.",
                Agendamento = new
                {
                    Aluno = aluno.Nome,
                    Plano = aluno.Plano.ToString(),
                    Aula = aula.Tipo,
                    DataHora = aula.DataHora
                }
            });
        }

        [HttpGet("por-aluno/{alunoId}")]
        public async Task<IActionResult> GetPorAluno(int alunoId)
        {
            var agendamentos = await _context.Agendamentos
                .Include(a => a.Aluno)
                .Include(a => a.Aula)
                .Where(a => a.AlunoId == alunoId)
                .ToListAsync();

            var dto = _mapper.Map<List<AgendamentoDto>>(agendamentos);
            return Ok(dto);
        }

        [HttpGet("por-aula/{aulaId}")]
        public async Task<IActionResult> GetPorAula(int aulaId)
        {
            var agendamentos = await _context.Agendamentos
                .Include(a => a.Aluno)
                .Include(a => a.Aula)
                .Where(a => a.AulaId == aulaId)
                .ToListAsync();

            var dto = _mapper.Map<List<AgendamentoDto>>(agendamentos);
            return Ok(dto);
        }

    }
}
