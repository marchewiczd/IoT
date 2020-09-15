using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace IoT_Api.Database
{
    public class LiteDb<T>
    {
        private string _connectionString;

        public LiteDb(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public void Insert(T data)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                db.GetCollection<T>().Insert(data);
            }
        }

        public void Delete(ulong id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                db.GetCollection<T>().Delete(id);
            }
        }

        public void DeleteAll()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                db.GetCollection<T>().DeleteAll();
            }
        }

        public List<T> GetAll()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<T>().FindAll().ToList();
            }
        }

        public T GetById(ulong id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<T>().FindById(id);
            }
        }

        public void UpdateData(T newData)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                db.GetCollection<T>().Update(newData);
            }
        }
    }
}
