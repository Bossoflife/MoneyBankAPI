using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Security.Principal;

namespace MoneyBankAPI.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AccountName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType  AccountType { get; set; } // This is an Enum will show if the account is a "savings" or a "current" account,
        public string AccountNumberGenerated { get; set; }  //We shall generate the accountNumber here !

        public byte[]? PinHash { get; set; }
        public byte[]? PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpated { get; set; }

        // lets generate an accountNumber we will do that with a constructor
        // first lets create a new object

        Random rand = new Random();

        public Account()
        {
            AccountNumberGenerated = Convert.ToString((long)rand.NextDouble() * 9_000_000_000 + 1_000_000_000);
            //we did a 9_000_000_00 so we cud generate a radom 10digit random number 
            AccountName = $"{FirstName}{LastName}";  
        }
    }


    public enum AccountType
    {
        Savings,
        Current,
        Corporate, 
        Government
    }
}
