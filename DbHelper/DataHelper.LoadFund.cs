using MigrationHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbHelper
{
    public partial class DataHelper
    {
        public async Task<bool> LoadFund(int userId, long accountnumber, double balance,
           int currencyId, DateTime currentDate, string remark)
        {
            var objUserAccount = new UserAccount
            {
                RegisterId = userId,
                AccNum = accountnumber,
                Balance = balance,
                CurrencyId = currencyId,
                CreatDate = currentDate,
                Remark = remark
            };

            try
            {
                await new Repository<UserAccount>(contxt).AddAsync(objUserAccount);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<List<UserAccount>> GetUserAccountByIdAsync(int userid)
        {
            var list = new List<UserAccount>();
            (await new Repository<UserAccount>(contxt).FindAsync(x => x.RegisterId == userid)).ToList().ForEach(x => list.Add(x));
            return list;
        }

    }
}
