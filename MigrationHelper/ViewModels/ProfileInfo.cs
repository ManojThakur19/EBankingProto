using MigrationHelper.Models;


namespace MigrationHelper.ViewModels
{
    public class ProfileInfo : MainModel
    {
        public string FullName { get; set; }
        public long AccNum { get; set; }
        public string AccountType { get; set; }
        public string Balance { get; set; }
    }
}
