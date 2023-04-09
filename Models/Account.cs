using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.Json.Serialization;

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
        public AccountType AccountType { get; set; } // This is an Enum will show if the account is a "savings" or a "current" account,
        public string AccountNumberGenerated { get; set; }  //We shall generate the accountNumber here !

        //this the PinHash and PinSalt to hash the password and it most 
        [JsonIgnore]
        public byte[] PinHash { get; set; }
        [JsonIgnore]
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpated { get; set; }

        // lets generate an accountNumber we will do that with a constructor
        // first lets create a new object

        readonly Random rand = new();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Account()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            AccountNumberGenerated = Convert.ToString((long)Math.Floor(rand.NextDouble() * 9_000_000_000 + 1_000_000_000));
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
