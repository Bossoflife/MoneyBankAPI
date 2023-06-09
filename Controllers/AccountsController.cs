﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyBankAPI.Models;
using MoneyBankAPI.Service.Implimentation;
using MoneyBankAPI.Service.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MoneyBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AccountsController : ControllerBase
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
        [Route("register_new_account")]
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
        public IActionResult GetAllAccounts()
        {
            var accounts = _accountService.GetAllAccount();
            var cleanedAccounts = _mapper.Map<IList<GetAccountModel>>(accounts);
            return Ok(cleanedAccounts);
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
        [HttpDelete]
        [Route("Remove_by_account_number")]
        public IActionResult GetByAccountNumber(string AccountNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Account Number most not be more than 10-digits");
            }
            var account = _accountService.GetByAccountNumber(AccountNumber);
            var cleanedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(cleanedAccount);
        }
        [HttpGet]
        [Route("get_account_By_Id")]
        public IActionResult GetByAccountById(int Id)
        {
            var account = _accountService.GetById(Id);
            var cleanedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(cleanedAccount);
        }
        [HttpPost]
        [Route("update_account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var account = _mapper.Map<Account>(model);

            _accountService.Update(account);
            return Ok();
        }


    }

}


