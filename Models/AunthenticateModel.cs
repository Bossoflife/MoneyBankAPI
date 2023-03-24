using System.ComponentModel.DataAnnotations;

namespace MoneyBankAPI.Models
{
    public class AunthenticateModel
    {
        [Required] // let's vailidate the account with 10-digit using Regex attribute
        [RegularExpression(@"^[0][1-9]\d{10}$|^{1-9}\d{9}$")]
        public string AccountNumber { get; set; }
        [Required]
        public string Pin { get; set; }
    }
}
