using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGymApi.Api.Data;
using MyGymApi.Api.DTOs;
using MyGymApi.Api.Models;
using AutoMapper;

namespace MyGymApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AulaController : ControllerBase
    {
        private readonly AcademiaContext _context;
        private readonly IMapper _mapper;

        public AulaController(AcademiaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var aulas = await _context.Aulas.ToListAsync();
            var aulasDto = _mapper.Map<List<AulaDto>>(aulas);
            return Ok(aulasDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);
            if (aula == null) return NotFound();

            var aulaDto = _mapper.Map<AulaDto>(aula);
            return Ok(aulaDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAulaDto dto)
        {
            var aula = _mapper.Map<Aula>(dto);
            _context.Aulas.Add(aula);
            await _context.SaveChangesAsync();

            var aulaDto = _mapper.Map<AulaDto>(aula);
            return CreatedAtAction(nameof(GetById), new { id = aula.Id }, aulaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAulaDto dto)
        {
            var aulaExistente = await _context.Aulas.FindAsync(id);
            if (aulaExistente == null) return NotFound();

            _mapper.Map(dto, aulaExistente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);
            if (aula == null) return NotFound();

            _context.Aulas.Remove(aula);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
