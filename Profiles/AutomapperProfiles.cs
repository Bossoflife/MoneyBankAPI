using AutoMapper;
using MoneyBankAPI.Models;

namespace MoneyBankAPI.Profiles
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<RegisterNewAccountModel, Account>();

            CreateMap<UpdateAccountModel, Account>();

            CreateMap<Account, GetAccountModel>();
            // We will create these dto classes
            CreateMap<Account, TransactionRequestDto>();
        }
    }
}
