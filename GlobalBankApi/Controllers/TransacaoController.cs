using Microsoft.AspNetCore.Mvc;
using GlobalBankApi.Data;
using GlobalBankApi.Models;

namespace GlobalBankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransacoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult RegistrarTransacao(Transacao transacao)
        {
            var conta = _context.Contas.Find(transacao.ContaId);

            if (conta == null)
                return NotFound("Conta não encontrada.");

            if (transacao.Tipo == "Saque" && transacao.Valor > conta.Saldo)
                return Conflict("Saldo Insuficiente");

            if (transacao.Tipo == "Deposito")
                conta.Saldo += transacao.Valor;
            else if (transacao.Tipo == "Saque")
                conta.Saldo -= transacao.Valor;

            if (transacao.Valor > 10000)
            {
                Console.WriteLine($"🚩 ALERTA DE SEGANÇA: Transação de alto valor detectada para a conta {conta.NumeroConta}!");
            }

            transacao.DataTransacao = DateTime.Now;

            _context.Transacoes.Add(transacao);
            _context.SaveChanges();

            return Ok(transacao);
        }

        [HttpGet("extrato/{contaId}")]
        public IActionResult Extrato(int contaId)
        {
            var extrato = _context.Transacoes
                .Where(t => t.ContaId == contaId)
                .ToList();

            return Ok(extrato);
        }
    }
}