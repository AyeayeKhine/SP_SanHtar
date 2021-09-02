

namespace SP_SanHtar.Interfaces
{
    using SP_SanHtar.CustomModels;
    using SP_SanHtar.Models;
    using SQLite;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    public interface IRepository<T> where T : class, new()
    {
        Task<UserModel> GetUser(string userName,string password);
        Task<List<MyanmarModel>> GetAllMyanmar();
        Task<MyanmarModel> GetMyanmar(Guid ID);
        Task<List<MyanmarModel>> GetTypeOfSubject(int TypeofSubject);
        Task<List<T>> Get();
        Task<T> Get(int id);
        Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        AsyncTableQuery<T> AsQueryable();
        Task<int> Insert(T entity);
        Task<int> Update(T entity);
        Task<int> Delete(T entity);
        Task<int> DeleteById(Guid ID);
        Task<List<MyanmarModel>> GetSearchResults(string title);
    }
}
