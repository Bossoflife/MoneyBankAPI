using System.ComponentModel.DataAnnotations;

namespace MoneyBankAPI.Models
{
    public class AunthenticateModel
    {
        [Required] // let's vailidate the account with 10-digit using Regex attribute
        public string AccountNumber { get; set; }
        [Required]
        public string Pin { get; set; }
    }
}
