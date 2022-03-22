using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZealandDimselab.Interfaces;
using ZealandDimselab.Models;
using ZealandDimselab.Services;

namespace ZealandDimselabTest
{
    [TestClass]
    public class ItemServiceTest
    {
        //private MockData _repositoryItem;
        private ItemService _itemService;
        private List<Item> _itemList;

        [TestInitialize]
        public void InitializeTest()
        {
            //_repositoryItem = new MockData<Item>();
            //_itemService = new ItemService(_repositoryItem, new ItemDbService());

            //_repositoryItem = new DimselabDbContext();
            //_itemService = new ItemService(_repositoryItem);
            _itemService = new ItemService();
            _itemList = _itemService.GetAllItems().Result.ToList();
        }

        [TestMethod]
        public void GetAllItems_Count() //GetAllItems is in InitializeTest
        {
            Assert.AreEqual(_itemList.Count, 5);
        }

        [TestMethod]
        public void GetAllItems_CorrectItems() //GetAllItems is in InitializeTest
        {
            Item firstItem = new Item("Test Item 1", "Test Description") { Id = 1 };
            Assert.IsTrue(firstItem.Equals(_itemList[0]));
            Item secondItem = new Item("Test Item 2", "Test Description") { Id = 2 };
            Assert.IsTrue(secondItem.Equals(_itemList[1]));
            Item thirdItem = new Item("Test Item 3", "Test Description") { Id = 3 };
            Assert.IsTrue(thirdItem.Equals(_itemList[2]));
            Item fourthItem = new Item("Test Item 4", "Test Description") { Id = 4 };
            Assert.IsTrue(fourthItem.Equals(_itemList[3]));
            Item fifthItem = new Item("Test Item 5", "Test Description") { Id = 5 };
            Assert.IsTrue(fifthItem.Equals(_itemList[4]));
        }

        [TestMethod]
        public async Task GetItemById_ValidId()
        {
            Item item = await _itemService.GetItemByIdAsync(1);
            Item firstItem = new Item("Test Item 1", "Test Description") { Id = 1 };

            Assert.IsTrue(firstItem.Equals(item));
        }

        [TestMethod]
        public async Task GetItemById_InvalidId()
        {
            Item item = await _itemService.GetItemByIdAsync(6);
            // item does not exist, so should have returned null
            Assert.IsNull(item);
        }

        [TestMethod]
        public async Task AddItemAsync_ValidItem()
        {
            Item newItem = new Item("Test Item 6", "Test Description");
            await _itemService.AddItemAsync(newItem);
            Item addedItem = _itemService.GetAllItems().Result.ToList().Last();

            Assert.AreEqual(newItem.Name, addedItem.Name);
            Assert.AreEqual(newItem.Description, addedItem.Description);
        }

        [TestMethod]
        public async Task DeleteItemAsync_ValidItemId()
        {
            int expectedCount = _itemService.GetAllItems().Result.ToList().Count - 1;
            await _itemService.DeleteItemAsync(5);
            int newCount = _itemService.GetAllItems().Result.ToList().Count;

            Assert.AreEqual(expectedCount, newCount);

            // Check that it has deleted the correct item
            Item firstItem = new Item("Test Item 1", "Test Description") { Id = 1 };
            Assert.IsTrue(firstItem.Equals(_itemList[0]));
            Item secondItem = new Item("Test Item 2", "Test Description") { Id = 2 };
            Assert.IsTrue(secondItem.Equals(_itemList[1]));
            Item thirdItem = new Item("Test Item 3", "Test Description") { Id = 3 };
            Assert.IsTrue(thirdItem.Equals(_itemList[2]));
            Item fourthItem = new Item("Test Item 4", "Test Description") { Id = 4 };
            Assert.IsTrue(fourthItem.Equals(_itemList[3]));
        }

        [TestMethod]
        public async Task UpdateItemAsync_ValidItem()
        {
            Item item = await _itemService.GetItemByIdAsync(5);
            item.Name = "New Name";

            await _itemService.UpdateItemAsync(item.Id, item);

            Assert.AreEqual(await _itemService.GetItemByIdAsync(5), item);
        }

        [TestMethod]
        public async Task UpdateItemAsync_NewItem_NoId()
        {
            Item item = new Item("New Item", "Test Description");

            await _itemService.UpdateItemAsync(0, item);

            // Items with an empty key get added to the database
            Assert.AreEqual(6, _itemService.GetAllItems().Result.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public async Task UpdateItemAsync_NewItem_WithId()
        {
            Item item = new Item("New Item", "Test Description") { Id = -1 };

            await _itemService.UpdateItemAsync(-1, item);

            // Items with a key that does not exist in the database should throw an exception
            Assert.AreEqual(6, _itemService.GetAllItems().Result.ToList().Count);
        }

        //internal class MockData : IItemDb
        //{
        //    DimselabDbContext dbContext;

        //    public MockData()
        //    {
        //        var options = new DbContextOptionsBuilder<DimselabDbContext>()
        //           .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //           .Options;
        //        dbContext = new DimselabDbContext(options);
        //        LoadDatabase();
        //    }

        //    public async Task<Item> GetItemWithCategoriesAsync(int id)
        //    {
        //        Item item;

        //        using (var context = new DimselabDbContext())
        //        {
        //            item = await context.Items
        //                .Include(i => i.Category)
        //                .Where(i => i.Id == id)
        //                .FirstOrDefaultAsync();
        //        }

        //        return item;
        //    }

        //    public async Task<List<Item>> GetAllItemsWithCategoriesAsync()
        //    {
        //        List<Item> items;

        //        using (var context = new DimselabDbContext())
        //        {
        //            items = await context.Items
        //                .Include(i => i.Category)
        //                .ToListAsync();


        //        }

        //        return items;
        //    }

        //    public async Task<List<Item>> GetItemsWithCategoryId(int id)
        //    {
        //        List<Item> items;

        //        using (var context = new DimselabDbContext())
        //        {
        //            items = await context.Items
        //                .Include(i => i.Category)
        //                .Where(i => i.CategoryId == id)
        //                .ToListAsync();
        //        }

        //        return items;
        //    }

        //    public async Task<IEnumerable<Item>> GetObjectsAsync()
        //    {
        //        return await dbContext.Set<Item>().AsNoTracking().ToListAsync();
        //    }

        //    public async Task<Item> GetObjectByKeyAsync(int id)
        //    {
        //        return await dbContext.Set<Item>().FindAsync(id);
        //    }

        //    public async Task AddObjectAsync(Item obj)
        //    {
        //        await dbContext.Set<Item>().AddAsync(obj);
        //        await dbContext.SaveChangesAsync();
        //    }

        //    public async Task DeleteObjectAsync(Item obj)
        //    {
        //        dbContext.Set<Item>().Remove(obj);
        //        await dbContext.SaveChangesAsync();
        //    }

        //    public async Task UpdateObjectAsync(Item obj)
        //    {
        //        dbContext.Set<Item>().Update(obj);
        //        await dbContext.SaveChangesAsync();
        //    }

        //public void DropDatabase()
        //    {
        //        dbContext.Database.EnsureDeleted();
        //    }

        //    private void LoadDatabase()
        //    {
        //        dbContext.Items.Add(new Item("Test Item 1", "Test Description"));
        //        dbContext.Items.Add(new Item("Test Item 2", "Test Description"));
        //        dbContext.Items.Add(new Item("Test Item 3", "Test Description"));
        //        dbContext.Items.Add(new Item("Test Item 4", "Test Description"));
        //        dbContext.Items.Add(new Item("Test Item 5", "Test Description"));

        //        dbContext.SaveChangesAsync();
        //    }
        //}
    }
}
