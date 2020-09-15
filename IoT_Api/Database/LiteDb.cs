using LiteDB;

namespace IoT_Api.Database
{
    public class LiteDb<DataType>
    {
        private string _connectionString;

        public LiteDb(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public void Insert(DataType data)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                db.GetCollection<DataType>().Insert(data);
            }
        }

        public void Delete(ulong id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                db.GetCollection<DataType>().Delete(id);
            }
        }

        public DataType SelectById(ulong id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<DataType>().FindById(id);
            }
        }

        public void UpdateData(DataType newData)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                db.GetCollection<DataType>().Update(newData);
            }
        }
    }
}
