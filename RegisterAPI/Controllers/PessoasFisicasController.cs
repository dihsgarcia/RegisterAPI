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
        
        [HttpPost("Create")]
        public async Task<IActionResult> PessoaFisicaCreate(CreatePessoaFisicaRequest request)
        {
            var id = await _service.CreateAsync(request);
            
            return CreatedAtAction( 
                nameof(GetByClienteId),
                new { clienteId = id },
                new { clienteId = id });
        }
        
        [HttpGet("GetByClienteId/{clienteId}")]
        public async Task<IActionResult> GetByClienteId(Guid clienteId)
        {
            var pessoaFisica = await _service.GetByIdAsync(clienteId);
            return Ok(pessoaFisica);
        }
        
        [HttpGet("GetByCpf/{cpf}")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var pessoaFisica = await _service.GetByCpfAsync(cpf);
            return Ok(pessoaFisica);
        }
        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(
            [FromBody] UpdatePessoaFisicaRequest request)
        {
            await _service.UpdateAsync(request);
            return NoContent();
        }
        
        [HttpDelete("Delete/{clienteId}")]
        public async Task<IActionResult> Delete(Guid clienteId)
        {
            await _service.DeleteAsync(clienteId);
            return NoContent();
        }
    }
}