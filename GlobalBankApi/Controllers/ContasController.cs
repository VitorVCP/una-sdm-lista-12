using Microsoft.AspNetCore.Mvc;
using GlobalBankApi.Data;
using GlobalBankApi.Models;

namespace GlobalBankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CriarConta(ContaBancaria conta)
        {
            if (conta.Saldo < 0)
                return BadRequest("O saldo inicial não pode ser negativo para contas internacionais.");

            _context.Contas.Add(conta);
            _context.SaveChanges();

            return Ok(conta);
        }

        [HttpGet]
        public IActionResult ListarContas()
        {
            return Ok(_context.Contas.ToList());
        }
    }
}