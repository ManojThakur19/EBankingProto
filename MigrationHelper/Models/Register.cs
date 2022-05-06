namespace MigrationHelper.Models
{
    public class Register: MainModel
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PinNumber { get; set; }
        public long AccNum { get; set; }
        public string AccountType { get; set; }
    }
}
