
namespace SP_SanHtar.Services
{
    using PCLStorage;
    using SP_SanHtar.Interfaces;
    using SP_SanHtar.CustomModels;
    using SQLite;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using SP_SanHtar.Models;
    using System.Linq;

    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private SQLiteAsyncConnection db;

        public Repository()
        {
            //this.db = db;
            db = GetConnection();
            db.CreateTableAsync<UserModel>();
            db.CreateTableAsync<MyanmarModel>();
        }

        public SQLite.SQLiteAsyncConnection GetConnection()
        {
            SQLiteAsyncConnection sqlitConnection;
            var sqliteFilename = "SPTeachingDB.db3";
            IFolder folder = FileSystem.Current.LocalStorage;
            string path = PortablePath.Combine(folder.Path.ToString(), sqliteFilename);
            sqlitConnection = new SQLite.SQLiteAsyncConnection(path);
            return sqlitConnection;
        }

        public AsyncTableQuery<T> AsQueryable() =>
            db.Table<T>();

        public async Task<UserModel> GetUser(string userName, string password)
        {
           //return await db.GetAsync<UserModel>(f => f.UserName == userName && f.Password == password);
            return await db.Table<UserModel>().FirstOrDefaultAsync(f => f.UserName == userName && f.Password == password);
        }

        public async Task<MyanmarModel> GetMyanmar(Guid ID)
        {
            //return await db.GetAsync<UserModel>(f => f.UserName == userName && f.Password == password);
            return await db.Table<MyanmarModel>().FirstOrDefaultAsync(f => f.ID == ID);
        }

        public async Task<List<MyanmarModel>> GetTypeOfSubject(int TypeofSubject)
        {
            //return await db.GetAsync<UserModel>(f => f.UserName == userName && f.Password == password);
            return await db.Table<MyanmarModel>().Where(f => f.typeOfSubject == TypeofSubject).ToListAsync();
        }

        public async Task<List<MyanmarModel>> GetAllMyanmar()
        {
            //return await db.GetAsync<UserModel>(f => f.UserName == userName && f.Password == password);
            return await db.Table<MyanmarModel>().ToListAsync();
        }

        public async Task<List<T>> Get() =>
            await db.Table<T>().ToListAsync();

        public async Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = db.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy<TValue>(orderBy);

            return await query.ToListAsync();
        }

        public async Task<T> Get(int id) =>
             await db.FindAsync<T>(id);

        public async Task<T> Get(Expression<Func<T, bool>> predicate) =>
            await db.FindAsync<T>(predicate);

        public async Task<int> Insert(T entity) =>
             await db.InsertAsync(entity);

        public async Task<int> Update(T entity) =>
             await db.UpdateAsync(entity);

        public async Task<int> Delete(T entity) =>
             await db.DeleteAsync(entity);

        public async Task<int> DeleteById(Guid ID) =>
            await db.Table<MyanmarModel>().DeleteAsync(x => x.ID == ID);

        //($"SELECT FileAS FROM  tblContacts WHERE FileAS LIKE '%{ keyword }%'").ToList();
        public async Task<List<MyanmarModel>> GetBychapter(int chapter)
        {
            var item = await db.QueryAsync<MyanmarModel>($"SELECT FileAS FROM  MyanmarModel WHERE FileAS LIKE '%{ chapter }%'");
            return item;
        }

        public async Task<List<MyanmarModel>> GetSearchResults(string title)
        {
            return await db.QueryAsync<MyanmarModel>($"SELECT * FROM  MyanmarModel WHERE Title LIKE '%{ title }%'");
        }
    }
}
