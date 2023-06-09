﻿using System.ComponentModel.DataAnnotations;

namespace MoneyBankAPI.Models
{
    public class RegisterNewAccountModel
    {
        // this class will have everything in the account 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpated { get; set; }
        //let's add regular expression
        [Required]
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "Pin do not match")]
        public string ConfirmPin { get; set; } // we want to compare both of them

    }
}
