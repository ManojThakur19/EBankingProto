using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationHelper.Models
{
    public class Transaction: MainModel
    {
        public Register FromUser { get; set; }
        public int FromUserId { get; set; }

        public Register ToUser { get; set; }
        public int ToUserId { get; set; }

        public long FromAccNum { get; set; }
        public long ToAccNum { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Remarks { get; set; }

        public Currency currency { get; set; }
        public int CurrencyId { get; set; }

    }
}
