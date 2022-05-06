using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationHelper.ViewModels
{
    public class TransactionSummaryViewModel
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public long FromAccount { get; set; }
        public int FromUserId { get; set; }
        public string FromFullName { get; set; }
        public string FromAccountType { get; set; }

        public long ToAccount { get; set; }
        public int ToUserId { get; set; }
        public string ToFullName { get; set; }
        public string ToAccountType { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
