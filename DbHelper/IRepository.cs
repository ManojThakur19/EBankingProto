using MigrationHelper.Models;

namespace DbHelper
{
    public interface IRepository<T> where T : MainModel
    {
        T Add(T item, bool saveChanges = true);
        T Update(T item,int id, bool saveChanges = true);


        void Remove(T item, bool saveChanges = true);
        void RemoveById(int id, bool saveChanges = true);
    }
}
