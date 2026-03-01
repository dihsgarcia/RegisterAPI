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
    }
}