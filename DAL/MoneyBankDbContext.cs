using Microsoft.EntityFrameworkCore;
using MoneyBankAPI.Models;

namespace MoneyBankAPI.DAL
{
    public class MoneyBankDbContext : DbContext
    {
        public MoneyBankDbContext(DbContextOptions<MoneyBankDbContext>options): base(options)
        {
             
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
 