using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigrationHelper.Models
{
    public class ExchangeRate : MainModel
    {
        //[Column(TypeName = "decimal(12,5)")]
        //public decimal GBP { get; set; }

        //[Column(TypeName = "decimal(12,5)")]
        //public decimal Euro { get; set; }

        //[Column(TypeName = "decimal(12,5)")]
        //public decimal USD { get; set; }

        public  Rate rates { get;set;}


    }
}
