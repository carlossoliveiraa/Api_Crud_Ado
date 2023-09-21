using Microsoft.AspNetCore.Mvc;
using Tier_Architecture.Application.Domain;
using Tier_Architecture.Application.Interfaces;

namespace Tier_Architecture.Presentation.Api.Controllers
{

    [Route("api/empresas")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaRepository _empresaRepository;

        public EmpresaController(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] Empresa empresa)
        {
            if (empresa == null)
            {
                return BadRequest("Empresa inválida");
            }

            await _empresaRepository.Adicionar(empresa);

            return CreatedAtAction(nameof(ObterPorId), new { id = empresa.Id }, empresa);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var empresa = await _empresaRepository.ObterPorId(id);
            if (empresa == null)
            {
                return NotFound();
            }

            return Ok(empresa);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var empresas = await _empresaRepository.ObterTodos();
            return Ok(empresas);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Empresa empresa)
        {
            if (empresa == null || empresa.Id != id)
            {
                return BadRequest("Dados da empresa inválidos");
            }

            var empresaExistente = await _empresaRepository.ObterPorId(id);
            if (empresaExistente == null)
            {
                return NotFound();
            }

            await _empresaRepository.Atualizar(empresa);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var empresa = await _empresaRepository.ObterPorId(id);
            if (empresa == null)
            {
                return NotFound();
            }

            await _empresaRepository.Remover(id);

            return NoContent();
        }    

    }
}
