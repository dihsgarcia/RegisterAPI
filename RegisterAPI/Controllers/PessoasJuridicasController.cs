using Application.DTOs.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RegisterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasJuridicasController : ControllerBase
    {
        private readonly IPessoaJuridicaService _service;

        public PessoasJuridicasController(IPessoaJuridicaService service)
        {
            _service = service;
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> PessoaJuridicaCreate(CreatePessoaJuridicaRequest request)
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
            var pessoaJuridica = await _service.GetByIdAsync(clienteId);
            return Ok(pessoaJuridica);
        }
        
        [HttpGet("GetByCnpj/{cnpj}")]
        public async Task<IActionResult> GetByCnpj(string cnpj)
        {
            var pessoaJuridica = await _service.GetByCnpjAsync(cnpj);
            return Ok(pessoaJuridica);
        }
        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(
            [FromBody] UpdatePessoaJuridicaRequest request)
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