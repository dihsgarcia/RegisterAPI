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
        public async Task<IActionResult> CreatePessoaFisica(CreatePessoaFisicaNewRequest request)
        {

            var teste = request;
            
            return Ok(request);
        }
        
        
        [HttpPost]
        public async Task<IActionResult> CreatePessoaFisica(CreatePessoaFisicaRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(new { id });
        }
        
        [HttpGet("buscar")]
        public async Task<IActionResult> GetByCpf([FromQuery] string cpf)
        {
            var pessoa = await _service.GetByCpfAsync(cpf);
            return Ok(pessoa);
        }

        [HttpPut("atualizar")]
        public async Task<IActionResult> Update(
            [FromQuery] string cpf,
            [FromBody] UpdatePessoaFisicaRequest request)
        {
            await _service.UpdateAsync(cpf, request);
            return NoContent();
        }

        [HttpDelete("deletar")]
        public async Task<IActionResult> Delete([FromQuery] string cpf)
        {
            await _service.DeleteAsync(cpf);
            return NoContent();
        }
    }
}