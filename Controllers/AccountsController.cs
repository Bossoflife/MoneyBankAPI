using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyBankAPI.Models;
using MoneyBankAPI.Service.Implimentation;
using MoneyBankAPI.Service.Interface;
using System.Collections.Generic;

namespace MoneyBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        //now let's inject the AccountService
        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            //let's also bring Automapper
            _accountService = accountService;
            _mapper = mapper;
        }
        // RegisterNewAccount
        [HttpPost]
        [Route("register_new_accout")]
        public IActionResult ResgisterNewAcount([FromBody] RegisterNewAccountModel newAccount)
        {
            if (!ModelState.IsValid) return BadRequest(newAccount);

            //map to account
            var account = _mapper.Map<Account>(newAccount);
            return Ok(_accountService.Create(account, newAccount.Pin, newAccount.ConfirmPin));

        }
        //GetAllAccount
        [HttpGet]
        [Route("get_all_accout")]
        public IAccountService GetAllAccounts()
        {
            var accounts = _accountService.GetAllAccount();
            var cleanedAccounts = _mapper.Map<IList<GetAccountModel>>(accounts);
            return (IAccountService)Ok(cleanedAccounts);
        }
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authnticate([FromBody] AunthenticateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            //now let's map
            return Ok(_accountService.Authenticate(model.AccountNumber, model.Pin));

            // it will return account... let's see when we run before we know whether to map it
        }

    }

}
