using MigrationHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHelper
{
    public partial class DataHelper
    {
        public async Task<Register> GetLoginUserAsync(string username, string pinnumber)
        {
            return (await new Repository<Register>(contxt).FindAsync(x => x.Username == username && x.PinNumber == pinnumber)).FirstOrDefault();
        }
    }
}
