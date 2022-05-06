using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigrationHelper.Models
{
    public class UserAccount : MainModel
    {
        public Register register { get; set; }
        public int RegisterId { get; set; }

        public long AccNum { get; set; }

        //[Column(TypeName= "decimal(12,5)")]
        public double Balance { get; set; }

        public Currency currency { get; set; }
        public int CurrencyId { get; set; }

        public DateTime CreatDate { get; set; }
        public string Remark { get; set; }
    }
}
