using Application.DTOs.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RegisterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasFisicasController : ControllerBase
    {
        
        [HttpPost]
        public async Task<IActionResult> CreatePessoaFisica(CreatePessoaFisicaRequest request)
        {
            var id = new Guid();
            return CreatedAtAction(nameof(CreatePessoaFisica), new { id }, new { id });
        }
    }
}