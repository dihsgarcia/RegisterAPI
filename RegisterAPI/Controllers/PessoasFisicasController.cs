using Application.DTOs.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RegisterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasFisicasController : ControllerBase
    {
        private readonly IPessoaFisicaService _service;

        public PessoasFisicasController(IPessoaFisicaService service)
        {
            _service = service;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreatePessoaFisica(CreatePessoaFisicaRequest request)
        {
            var id = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(CreatePessoaFisica), new { id }, new { id });
        }
        
        [HttpGet("{cpf}")]
        public async Task<IActionResult> GetByCpf([FromRoute] string cpf)
        {
            var pessoa = await _service.GetByCpfAsync(cpf);
            return Ok(pessoa);
        }

        [HttpPut("{cpf}")]
        public async Task<IActionResult> Update( 
            [FromRoute] string cpf,
            [FromBody] UpdatePessoaFisicaRequest request)
        {
            await _service.UpdateAsync(cpf, request);
            return NoContent();
        }

        [HttpDelete("{cpf}")]
        public async Task<IActionResult> Delete([FromRoute] string cpf)
        {
            await _service.DeleteAsync(cpf);
            return NoContent();
        }
    }
}