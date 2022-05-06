
using MigrationHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbHelper
{
    public partial class DataHelper
    {
        public async Task<bool> RegisterUser(string username, string firstname, string lastname,
            string pinNumber, long accountNumber, string accountType)
        {
            var objRegister = new Register
            {
                Username = username,
                Firstname = firstname,
                Lastname = lastname,
                PinNumber = pinNumber,
                AccNum = accountNumber,
                AccountType = accountType
            };

            try
            {
                await new Repository<Register>(contxt).AddAsync(objRegister);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<List<Register>> GetUsersAsync()
        {
            var list = new List<Register>();

            (await new Repository<Register>(contxt).FindAllAsync()).ToList().ForEach(x => list.Add(x));

            return list;
        }

        public async Task<Register> GetUserByIdAsync(int id) 
        {
            return (await new Repository<Register>(contxt).FindByIdAsync(id));
        }

        public async Task<Register> GetUserByAccountNumberAsync(long accountNumber)
        {
            return (await new Repository<Register>(contxt).FindAsync(x => x.AccNum == accountNumber)).FirstOrDefault();
        }
    }
}
