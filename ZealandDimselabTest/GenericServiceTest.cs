using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZealandDimselab.Models;
using ZealandDimselab.Services;

namespace ZealandDimselabTest
{
    [TestClass]
    public class GenericServiceTest<T>
    {
        private GenericService<Item> _genericItemService;
        private GenericService<User> _genericUserService;
        private Item item;
        private User user;


        [TestInitialize]
        public void InitializeTest()
        {
            IDbService<Item> itemDbService = new GenericDbService<Item>();
            _genericItemService = new GenericService<Item>(itemDbService);

            IDbService<User> userDbService = new GenericDbService<User>();
            _genericUserService = new GenericService<User>(userDbService);


            item = new Item() { CategoryId = 1, Description = "Test Item Description", Name = "Test Item"};
        }

        [TestMethod]
        public async Task Add_ValidItem()
        {
            int currentCount = _genericItemService.GetAllObjects().Count;

            await _genericItemService.AddObjectAsync(item);

            int newCount = _genericItemService.GetAllObjects().Count;


            Assert.AreEqual(currentCount +1, newCount);
        }

}
}
