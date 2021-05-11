using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZealandDimselab.Services
{
    public interface IDbService<T>
    {
        Task<IEnumerable<T>> GetObjectsAsync();
        Task<T> GetObjectByKeyAsync(int id);
        Task AddObjectAsync(T obj);
        Task DeleteObjectAsync(T obj);
        Task UpdateObjectAsync(T obj);
    }
}
