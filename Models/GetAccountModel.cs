using System.ComponentModel.DataAnnotations;

namespace MoneyBankAPI.Models
{
    public class GetAccountModel
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
        public string? AccountNumberGenerated { get; set; }  //We shall generate the accountNumber here !

        //this the PinHash and PinSalt to hash the password and it most not be more than 4-Digit!!!
        public byte[]? PinHash { get; set; }
        public byte[]? PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpated { get; set; }


    }
}



