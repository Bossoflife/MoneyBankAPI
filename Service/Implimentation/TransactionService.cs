using Microsoft.Extensions.Options;
using MoneyBankAPI.DAL;
using MoneyBankAPI.Models;
using MoneyBankAPI.Service.Interface;
using MoneyBankAPI.Utils;
using Newtonsoft.Json;

namespace MoneyBankAPI.Service.Implimentation
{
    public class TransactionService : ITransactionService
    {
        private readonly MoneyBankDbContext _dbContext;
        private readonly ILogger<TransactionService> _logger;
        private readonly Appsettings _settings;
        private static string _ourBankSettlementAccount;
        private readonly IAccountService _accountService;

        public TransactionService(MoneyBankDbContext dbContext, ILogger<TransactionService> logger, IOptions<Appsettings> settings, IAccountService accountService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _settings = settings.Value;
            _ourBankSettlementAccount = _settings.OurBankSettlementAccount;
            _accountService = accountService;
        }
        public Response CreateNewTransaction(Transaction transaction)
        {
            Response response = new Response();
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created succesfully!";
            response.Data = null;

            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction = _dbContext.Transactions.Where(x => x.TransactionDate == date).ToList();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully";
            response.Data = transaction;

            return response;
        }

        public Response MakeDeposite(string AccountNumber, decimal Amount, string TransactionPin)
        {// let's deposit into our account....
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //first we going to check the user if the account is vaild,
            //we wil need aunthentication in UserService, so let's inject IAccountService 
            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalikd credentials");

            try
            {
                //for deposit, ourbankSettlementAccout is th source giving the money to the User's Account
                sourceAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);
                destinationAccount = _accountService.GetByAccountNumber(AccountNumber);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                    &&
                    (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error Occured.... => {ex.Message}");
            }
            //set other prop of transaction here
            transaction.TransactionType = TranType.Deposit;
            transaction.TransactionSourceAccount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionAmount = Amount.ToString();
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"New Transaction From Source {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $" To Destination Account => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                $" On Date => {JsonConvert.SerializeObject(transaction.TransactionDate)}" +
                $" For Amount => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                $" Transaction Type => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $" Transaction Status => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            return response;
        }

        public Response MakeFundsTransfer(string FromAccount, string TransactionPin, decimal Amount, string ToAccount)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //first we going to check the user if the account is vaild,
            //we wil need aunthentication in UserService, so let's inject IAccountService 
            var authUser = _accountService.Authenticate(FromAccount, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalikd credentials");

            try
            {
                //for deposit, ourbankSettlementAccout is th destinationAccount getting the money from the Account
                sourceAccount = _accountService.GetByAccountNumber(FromAccount);
                destinationAccount = _accountService.GetByAccountNumber(ToAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                    &&
                    (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error Occured.... => {ex.Message}");
            }
            //set other prop of transaction here
            transaction.TransactionType = TranType.Withdrawal;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount.ToString();
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"New Transaction From Source {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $" To Destination Account => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                $" On Date => {JsonConvert.SerializeObject(transaction.TransactionDate)}" +
                $" For Amount => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                $" Transaction Type => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $" Transaction Status => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            return response;
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            // now let's make a withdrawal....!
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //first we going to check the user if the account is vaild,
            //we wil need aunthentication in UserService, so let's inject IAccountService 
            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalikd credentials");

            try
            {
                //for deposit, ourbankSettlementAccout is th destinationAccount getting the money from the Account
                sourceAccount = _accountService.GetByAccountNumber(AccountNumber);
                destinationAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                    &&
                    (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error Occured.... => {ex.Message}");
            }
            //set other prop of transaction here
            transaction.TransactionType = TranType.Withdrawal;
            transaction.TransactionSourceAccount = AccountNumber;
            transaction.TransactionDestinationAccount = _ourBankSettlementAccount;
            transaction.TransactionAmount = Amount.ToString();
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"New Transaction From Source {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $" To Destination Account => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                $" On Date => {JsonConvert.SerializeObject(transaction.TransactionDate)}" +
                $" For Amount => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                $" Transaction Type => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $" Transaction Status => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            return response;
        }

    }

}
