using System.ComponentModel.DataAnnotations;

namespace MoneyBankAPI.Models
{
    public class UpdateAccountModel
    {
        [Key]
        public int Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must not be more that 4 digits")] //it shoud be 4-digits string
        public string? Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "Pin do not match")]
        public string? ConfirmPin { get; set; } // we want to compare both of them

        public DateTime DateLastUpdate { get; set; }
    }
}
