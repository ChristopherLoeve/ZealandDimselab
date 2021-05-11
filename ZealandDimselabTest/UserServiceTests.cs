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
    public class UserServiceTests
    {
        private IDbService<User> repository;
        private UserService userService;

        [TestInitialize]
        public void InitializeTest()
        {
            repository = new UserMockData<User>();
            userService = new UserService(repository);
        }

        [TestMethod]
        public void GetUsers_Default_ReturnsAllUsers()
        {
            // Arrange
            var expectedCount = 5;

            // Act
            var actualCount = userService.GetUsersAsync().ToList().Count;

            // Assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public async Task AddUserAsync_AddUser_IncrementCount()
        {
            // Arrange
            var expectedCount = 6;
            User user = new User("Mike", "Mike@gmail.com", "Mike1234");
            await userService.AddUserAsync(user);

            // Act
            var actualCount = userService.GetUsersAsync().ToList().Count;
            // Assert
            Assert.AreEqual(expectedCount, actualCount);
           
        }
        [TestMethod]
        public async Task AddUserAsync_AddUser_HashesPassword()
        {
            // Arrange
            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            string passwordIsNot = "Mike1234";
            User user = new User("Mike", "Mike@gmail.com", "Mike1234");
            // Act
            await userService.AddUserAsync(user);
            user = await userService.GetUserByIdAsync(6);
            // Assert
            Assert.AreNotEqual(user.Password, passwordIsNot);
        }
        [TestMethod]
        public async Task DeleteUserAsync_RemovesUser_DecreasesCount()
        {
            // Arrange
            var expectedCount = 4;
            var id = 1;
            await userService.DeleteUserAsync(id);
            // Act
            int actualCount = userService.GetUsersAsync().ToList().Count;

            // Assert
            Assert.AreEqual(expectedCount, actualCount);

        }
        [TestMethod]
        public async Task GetUserByIdAsync_ValidId_ReturnsUserObject()
        {
            string expectedname = "Oscar";
            string expectedEmail = "Oscar@gmail.com";

            // Act
            User actualUser = await userService.GetUserByIdAsync(3);

            // Assert
            Assert.AreEqual(expectedname, actualUser.Name);
            Assert.AreEqual(expectedEmail, actualUser.Email);
        }

        [TestMethod]
        public async Task UpdateUserAsync_UpdateExsitingUser_ReturnsUpdatedObject()
        {
            // Arrange
            User user = await userService.GetUserByIdAsync(3); 
            string expectedName = "Hoscar";
            string expectedEmail = "Hoscar@gmail.com";

            // Act
            user.Name = expectedName;
            user.Email = expectedEmail;
            await userService.UpdateUserAsync(user);
            User actualUser = await userService.GetUserByIdAsync(3);

            // Assert

            Assert.AreEqual(expectedEmail, actualUser.Email);
            Assert.AreEqual(expectedName, actualUser.Name);
        }

        [TestMethod]
        public void ValidateLogin_ValidLogin_ReturnsTrue()
        {
            // Arrange
            string correctEmail = "Steven@gmail.com";
            string correctPassword = "Steven1234";
            bool expectedLoginResult;
            // Act
            expectedLoginResult = userService.ValidateLogin(correctEmail, correctPassword);

            // Assert

            Assert.IsTrue(expectedLoginResult);

        }

        [TestMethod]
        public void ValidateLogin_InvalidPasswordLogin_ReturnsFalse()
        {
            // Arrange
            string correctEmail = "Steven@gmail.com";
            string inCorrectPassword = "StevenIncorrect";
            bool expectedLoginResult;
            // Act
            expectedLoginResult = userService.ValidateLogin(correctEmail, inCorrectPassword);

            // Assert

            Assert.IsFalse(expectedLoginResult);

        }

        [TestMethod]
        public void ValidateLogin_InvalidEmailLogin_ReturnsFalse()
        {
            // Arrange
            string inCorrectEmail = "Steven@outlook.com";
            string correctPassword = "Steven1234";
            bool expectedLoginResult;
            // Act
            expectedLoginResult = userService.ValidateLogin(inCorrectEmail, correctPassword);

            // Assert

            Assert.IsFalse(expectedLoginResult);

        }

        [TestMethod]
        public void CreateClaim_ValidEmail_ReturnsClaimIdentity()
        {
            // Arrange
            string expectedClaimName = "Steven@outlook.com";
            // Act
            ClaimsIdentity actualClaimIdentity = userService.CreateClaimIdentity(expectedClaimName);

            // Assert

            Assert.AreEqual(expectedClaimName, actualClaimIdentity.Name);

        }

        [TestMethod]
        public void CreateClaim_LoginAsAdmin_AddsAdminRoleToClaim()
        {
            // Arrange
            string expectedRole = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role: admin"; // Why does it append schemas? Is it for intergration with AD?
            string email = "Admin@Dimselab.dk";
            ClaimsIdentity actualClaimIdentity = userService.CreateClaimIdentity(email);
            // Act
            string actualRole = actualClaimIdentity.Claims.FirstOrDefault(role => role.Value == "admin").ToString();

            // Assert
            Assert.AreEqual(expectedRole, actualRole);
        }

        [TestMethod]
        public void CreateClaim_LoginAsUser_DoesNotAddAdminRoleToClaim()
        {
            // Arrange
            string email = "Steven@gmail.com";
            ClaimsIdentity actualClaimIdentity = userService.CreateClaimIdentity(email);
            // Act
            Claim claimRole = actualClaimIdentity.Claims.FirstOrDefault(role => role.Value == "admin");
            // Assert
            Assert.IsNull(claimRole);
        }
    }
    internal class UserMockData<T> : IDbService<T> where T : class
    {
        private static List<User> _users;
        private readonly PasswordHasher<string> passwordHasher;
        DimselabDbContext dbContext;

        public UserMockData()
        {
            passwordHasher = new PasswordHasher<string>();

            var options = new DbContextOptionsBuilder<DimselabDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            dbContext = new DimselabDbContext(options);
            LoadDatabase();
        }

        public async Task AddObjectAsync(T obj)
        {
            await dbContext.Set<T>().AddAsync(obj);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteObjectAsync(T obj)
        {

            dbContext.Set<T>().Remove(obj);
            await dbContext.SaveChangesAsync();
        }

        public async Task<T> GetObjectByKeyAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetObjectsAsync()
        {
            return await dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task UpdateObjectAsync(T obj)
        {
            dbContext.Set<T>().Update(obj);
            await dbContext.SaveChangesAsync();

        }

        public void DropDatabase()
        {
            dbContext.Database.EnsureDeleted();
        }

        private void LoadDatabase()
        {
            dbContext.Users.Add(new User("Steven", "Steven@gmail.com", PasswordEncrypt("Steven1234")));
            dbContext.Users.Add(new User("Mikkel", "Mikkel@gmail.com", PasswordEncrypt("Mikkel1234")));
            dbContext.Users.Add(new User("Oscar", "Oscar@gmail.com", PasswordEncrypt("Oscar1234")));
            dbContext.Users.Add(new User("Christopher", "Christopher@gmail.com", PasswordEncrypt("Christopher1234")));
            dbContext.Users.Add(new User("Admin", "Admin@Dimselab", PasswordEncrypt("Admin1234")));

            dbContext.SaveChangesAsync();
        }

        private string PasswordEncrypt(string password)
        {
            return passwordHasher.HashPassword(null, password);
        }
    }
}
