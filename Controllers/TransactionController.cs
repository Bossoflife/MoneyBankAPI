using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoneyBankAPI.Models;
using MoneyBankAPI.Service.Implimentation;
using MoneyBankAPI.Service.Interface;

namespace MoneyBankAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {



        private readonly ITransactionService _transactionService;
        readonly IMapper _mapper;


        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        //create new transaction
        [HttpPost]
        [Route("create_new_transaction")]
        public IAccountService CreateNewTransaction([FromBody] TransactionRequestDto transactionRequest)
        {
            //but we cannot attach a Transaction Model because it has stuffs that the user dosen't have to fill
            //so instead let's create a transactionRequestDto and map it to the Transaction now, lets create a mapper in our Automapper
            if (!ModelState.IsValid) return (IAccountService)BadRequest(transactionRequest);

            var transaction = _mapper.Map<Transaction>(transactionRequest);
            return (IAccountService)Ok(_transactionService.CreateNewTransaction(transaction));
        }
        [HttpPost]
        [Route("create_new_deposit")]
        public IActionResult MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!ModelState.IsValid) return BadRequest("Deposit didn't go through");
            return Ok(_transactionService.MakeDeposite(AccountNumber, Amount, TransactionPin));
        }
        [HttpPost]
        [Route("create_new_withdrawal")]
        public IActionResult MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!ModelState.IsValid) return BadRequest("Withdrawal didn't go through please try again");
            return Ok(_transactionService.MakeWithdrawal(AccountNumber, Amount, TransactionPin));
        }
        [HttpPost]
        [Route("make_funds_transfer")]
        public IActionResult MakeFundsTransfer(string FromAccount, string TransactionPin, decimal Amount, string ToAccount)
        {
            if (ModelState.IsValid) return BadRequest("Sorry your transfer didn't go through");
            return Ok(_transactionService.MakeFundsTransfer(FromAccount, ToAccount, Amount, TransactionPin));
        }

    }
}
