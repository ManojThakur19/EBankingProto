using System.ComponentModel.DataAnnotations.Schema;


namespace MigrationHelper.Models
{
    public class Rate: MainModel
    {
        public int EUR { get; set; }
        public double GBP { get; set; }
        public double USD { get; set; }
    }
}
