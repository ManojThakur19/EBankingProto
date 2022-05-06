using MigrationHelper.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DbHelper
{
    public partial class DataHelper
    {
        public async Task<bool> AddTransaction(int fromUserId,int toUserId, long fromAccNo,
           long toAccNo, double amount, string remark ,int currencyId)
        {
            var objTransaction = new Transaction
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                FromAccNum = fromAccNo,
                ToAccNum = toAccNo,
                Amount = amount,
                Remarks = remark,
                CurrencyId = currencyId,
                Date = DateTime.Now
            };

            try
            {
                await new Repository<Transaction>(contxt).AddAsync(objTransaction);

                double finalVal = 0;
                finalVal = finalVal - amount;
                //add from old user
                var objUserAccount = new UserAccount
                {
                    RegisterId = fromUserId,
                    AccNum = fromAccNo,
                    CurrencyId = currencyId,
                    CreatDate = DateTime.Now,
                    Remark = remark,
                    Balance = finalVal
                };

                await new Repository<UserAccount>(contxt).AddAsync(objUserAccount);

                //add to new trasfered user
                
                var objUserAccounts = new UserAccount
                {
                    RegisterId = toUserId,
                    AccNum = toAccNo,
                    CurrencyId = currencyId,
                    CreatDate = DateTime.Now,
                    Remark = remark,
                    Balance = amount
                };

                await new Repository<UserAccount>(contxt).AddAsync(objUserAccounts);

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public async Task<List<Transaction>> GetTransaction()
        {
            var list = new List<Transaction>();
            (await new Repository<Transaction>(contxt).FindAllAsync()).ToList().ForEach(x => list.Add(x));
            return list;
        }
    }
}
