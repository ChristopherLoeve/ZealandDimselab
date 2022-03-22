using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealandDimselab.Interfaces;
using ZealandDimselab.Models;

namespace ZealandDimselab.Services
{
    public class GenericDbService<T> : IDbService<T> where T : class
    {
        /// <summary>
        /// Gets all objects of the type from the database
        /// </summary>
        /// <returns>Returns all objects in the Set of the given type as List</returns>
        public virtual async Task<IEnumerable<T>> GetObjectsAsync()
        {
            await using (var context = new DimselabDbContext())
            {
                return await context.Set<T>().AsNoTracking().ToListAsync();
            }
        }

        /// <summary>
        /// Finds object with the specified key in the database and returns it
        /// </summary>
        /// <param name="key">Key of the object you wish to find</param>
        /// <returns>Object with the matching key</returns>
        public virtual async Task<T> GetObjectByKeyAsync(int key)
        {
            await using (var context = new DimselabDbContext())
            {
                return await context.Set<T>().FindAsync(key);
            }
        }



        /// <summary>
        /// Adds the given object to the database for object type
        /// </summary>
        /// <param name="obj">The object you want to add. Has to be the correct type</param>
        /// <returns>async task</returns>
        public virtual async Task AddObjectAsync(T obj)
        {
            await using (var context = new DimselabDbContext())
            {
                await context.Set<T>().AddAsync(obj);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes given object from the database for object type
        /// </summary>
        /// <param name="obj">The object you want to delete</param>
        /// <returns>async Task</returns>
        public virtual async Task DeleteObjectAsync(T obj)
        {
            await using (var context = new DimselabDbContext())
            {
                
                context.Set<T>().Remove(obj);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates the given object in the database
        /// </summary>
        /// <param name="obj">The updated object</param>
        /// <returns>async Task</returns>
        public virtual async Task UpdateObjectAsync(T obj)
        {
            await using (var context = new DimselabDbContext())
            {
                context.Set<T>().Update(obj);
                await context.SaveChangesAsync();
            }
        }
    }
}
