using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealandDimselab.Models;
using ZealandDimselab.Interfaces;

namespace ZealandDimselab.Services
{
    public class ItemDbService : GenericDbService<Item>, IItemDb
    {
        //public async Task<Item> GetItemWithCategoriesAsync(int id)
        //{
        //    Item item;

        //    using (var context = new DimselabDbContext())
        //    {
        //        item = await context.Items
        //            .Include(i => i.Category)
        //            .Where(i => i.Id == id)
        //            .FirstOrDefaultAsync();
        //    }

        //    return item;
        //}

        //public async Task<List<Item>> GetAllItemsWithCategoriesAsync()
        //{
        //    List<Item> items;

        //    using (var context = new DimselabDbContext())
        //    {
        //        items = await context.Items
        //            .Include(i => i.Category)
        //            .ToListAsync();


        //    }

        //    return items;
        //}

        //public async Task<List<Item>> GetItemsWithCategoryId(int id)
        //{
        //    List<Item> items;

        //    using (var context = new DimselabDbContext())
        //    {
        //        items = await context.Items
        //            .Include(i => i.Category)
        //            .Where(i => i.CategoryId == id)
        //            .ToListAsync();
        //    }

        //    return items;
        //}
        public Task<List<Item>> GetAllItemsWithCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Item>> GetItemsWithCategoryId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Item> GetItemWithCategoriesAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
