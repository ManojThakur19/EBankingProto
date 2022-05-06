using MigrationHelper.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbHelper
{
    public interface IDataHelper: IDisposable
    {
        #region register
        Task<bool> RegisterUser(string username, string firstname, string lastname,
          string pinNumber, long accountNumber, string AccountType);

        Task<List<Register>> GetUsersAsync();

        Task<Register> GetUserByIdAsync(int id);

        Task<Register> GetUserByAccountNumberAsync(long accountNumber);
        #endregion

        #region login
        Task<Register> GetLoginUserAsync(string username, string pinnumber);
        #endregion

        #region currency
        Task<List<Currency>> GetCurrency();

        Task<List<Rate>> GetRate();

        Task<bool> DeleteRateById(int id);

        Task<bool> AddRate(int eur, double gbp, double usd);
        #endregion

        #region loadfund
        Task<bool> LoadFund(int userId, long accountnumber, double balance,
           int currencyId, DateTime currentDate, string remark);

        Task<List<UserAccount>> GetUserAccountByIdAsync(int userid);
        #endregion

        #region transaction
        Task<bool> AddTransaction(int fromUserId,int toUserId, long fromAccNo,
           long toAccNo, double amount, string remark, int currencyId);

        Task<List<Transaction>> GetTransaction();

        #endregion
    }
}