using MoneyBankAPI.DAL;
using MoneyBankAPI.Models;
using MoneyBankAPI.Service.Interface;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace MoneyBankAPI.Service.Implimentation
{
    public class AccountService : IAccountService
    {
        private readonly MoneyBankDbContext _dbContext;

        public AccountService(MoneyBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Account Authenticate(string AccountNumber, string Pin)
        {
            // let's make authenticate
            // dose account exist for that number 
            var account = _dbContext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefault();
            if (account == null)
                return null;
            // Ok so we have a match

            //verify pinHash
            if (!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
                return null;

            //Ok so Aunthentication is passed here
            return account;
        }
        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentNullException(nameof(Pin));
            //now let's verify pin
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i]) return false;

                }
            }
            return true;
        }
        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            // this is to create a new account
            if (_dbContext.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("An account already exist with this email");
            // this is to validate the email

            // this is to confirmPin with the already existing Pin of the account
            if (!Pin.Equals(ConfirmPin)) throw new ArgumentException("Pin do not match", nameof(Pin));

            // new validation pass, if Ok then let's create account,
            //we are hashing /encryting pin first,

            CreatePinHash(Pin, out byte[] pinHash, out byte[] pinSalt); // let's create this cryto method


            account.PinHash = pinHash;
            account.PinSalt = pinSalt;

            // all good new account to be added to the db
            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();


            return account;
        }
        public static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        public void Delete(int Id)
        {
            var account = _dbContext.Accounts.Find(Id);
            if (account != null)
            {
                _dbContext.Accounts.Remove(account);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<Account> GetAllAccount()
        {
            return _dbContext.Accounts.ToList();
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            var account = _dbContext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null)
            {
                return null;
            }
            return account;
        }

        public Account GetById(int Id)
        {
            var account = _dbContext.Accounts.Where(x => x.Id == Id).FirstOrDefault();
            if (account == null)
            {
                return null;
            }
            return account;
        }

        public void Update(Account account, string? Pin = null)
        {
            // Update is more tasky

            // actually we will allow th user to be able to change his Email, PhoneNumber and Pin 
            var accountToUpdated = _dbContext.Accounts.Where(x => x.Email == account.Email).SingleOrDefault();
            if (accountToUpdated == null) throw new ApplicationException("Account dose not exist please provide a vaild account");
            // if the account exists, lrt's listen for user to change any of his properties 
            if (!string.IsNullOrWhiteSpace(account.Email))
            {
                // this means the user has and account and wishes to change his/her email
                //check if the one he has changed to is already taken
                if (_dbContext.Accounts.Any(x => x.Email == account.Email))
                {
                    throw new ApplicationException("this Email" + account.Email + "already taken");
                }
                else
                {
                    //else change email 
                    accountToUpdated.Email = account.Email;
                }


            }
            // actually we will allow th user to be able to change his Email, PhoneNumber and Pin 
            // if the account exists, lrt's listen for user to change any of his properties 
            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                // this means the user has and account and wishes to change his/her PhoneNumber
                //check if the one he has changed to is already taken
                if (_dbContext.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber))
                {
                    throw new ApplicationException("PhoneNumber already taken");
                }
                else
                {
                    //else change PhoneNumber 
                    accountToUpdated.PhoneNumber = account.PhoneNumber;
                }


            }

            // actually we want to allow the user to be able to change his pin
            if (!string.IsNullOrWhiteSpace(Pin))
            {
                //this means the user wishes to change his/her Pin
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);

                accountToUpdated.PinHash = pinHash;
                accountToUpdated.PinSalt = pinSalt;
            }

        }


    }
}
