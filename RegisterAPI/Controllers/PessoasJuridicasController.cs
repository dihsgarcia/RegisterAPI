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
        
        /*[HttpPost]
        public async Task<IActionResult> CreatePessoaJuridica(CreatePessoaJuridicaRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(new { id });
        }
        
        [HttpGet("buscar")]
        public async Task<IActionResult> GetByCnpj([FromQuery] string cnpj)
        {
            var pessoa = await _service.GetByCnpjAsync(cnpj);
            return Ok(pessoa);
        }
        
        [HttpPut("atualizar")]
        public async Task<IActionResult> Update(
            [FromQuery] string cnpj,
            [FromBody] UpdatePessoaJuridicaRequest request)
        {
            await _service.UpdateAsync(cnpj, request);
            return NoContent();
        }
        
        [HttpDelete("deletar")]
        public async Task<IActionResult> Delete([FromQuery] string cnpj)
        {
            await _service.DeleteAsync(cnpj);
            return NoContent();
        }*/
    }
}