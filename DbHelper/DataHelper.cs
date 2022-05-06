using MigrationHelper.Data;

namespace DbHelper
{
    public partial class DataHelper: IDataHelper
    {
        private readonly ApplicationDbContext contxt;

        public DataHelper(ApplicationDbContext context)
        {
            contxt = context;
        }
        
        public void Dispose()
        {
            contxt.Dispose();
        }


    }
}
