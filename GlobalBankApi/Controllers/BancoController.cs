using Microsoft.AspNetCore.Mvc;
using GlobalBankApi.Data;

namespace GlobalBankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BancoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BancoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            var patrimonioTotal = _context.Contas.Sum(c => c.Saldo);
            var totalTransacoes = _context.Transacoes.Count();

            return Ok(new
            {
                PatrimonioTotal = patrimonioTotal,
                TotalTransacoes = totalTransacoes
            });
        }
    }
}