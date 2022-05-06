using MigrationHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbHelper
{
    public partial class DataHelper
    {
        public async Task<List<Currency>> GetCurrency()
        {
            var list = new List<Currency>();
            (await new Repository<Currency>(contxt).FindAllAsync()).ToList().ForEach(x => list.Add(x));
            return list;
        }

        public async Task<List<Rate>> GetRate()
        {
            var list = new List<Rate>();
            (await new Repository<Rate>(contxt).FindAllAsync()).ToList().ForEach(x => list.Add(x));
            return list;
        }

        public async Task<bool> DeleteRateById(int id)
        {
            if (id < 1)
            {
                return false;
            }
            try
            {
                new Repository<Rate>(contxt).RemoveById(id);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public async Task<bool> AddRate(int eur, double gbp, double usd)
        {
            var objRate = new Rate
            {
                EUR = eur,
                GBP = gbp,
                USD = usd
            };

            try
            {
                await new Repository<Rate>(contxt).AddAsync(objRate);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

    }
}
