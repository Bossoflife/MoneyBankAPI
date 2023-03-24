using Microsoft.AspNetCore.Http.Connections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace MoneyBankAPI.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public string? TransactionUniqueReference { get; set; }
        public string? TransactionAmount { get; set; }
        public TranStatus TransactionStatus { get; set; }
        public bool IsSuccessful => TransactionStatus.Equals(TranStatus.Success);
        public string? TransactionSourceAccount { get; set; }
        public string? TransactionDestinationAccount { get; set; }
        public string? TransactionParticulars { get; set; }
        public TranType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }

        public Transaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 27)}";
        }
    }
    public enum TranStatus
    {
        Failed,
        Success,
        Error
    }
    public enum TranType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
