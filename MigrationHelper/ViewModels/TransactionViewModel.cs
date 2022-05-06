using MigrationHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationHelper.ViewModels
{
    public class TransactionViewModel: MainModel
    {
        public Register FromRegister { get; set; }
        public int FromUserId { get; set; }

        public string FromName { get; set; }
        public double Balance { get; set; }
        
        public Register ToRegister { get; set; }
        public int ToUserId { get; set; }

        public long FromAccNum { get; set; }
        public long ToAccNum { get; set; }
        public long Amount { get; set; }
        public DateTime Date { get; set; }
        public string Remarks { get; set; }

        public Currency currency { get; set; }
        public int CurrencyId { get; set; }
    }
}
