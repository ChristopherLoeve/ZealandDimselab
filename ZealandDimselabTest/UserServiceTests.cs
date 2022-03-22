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
    public class UserServiceTests
    {
        private IUserDb repository;
        private UserService userService;

        [TestInitialize]
        public void InitializeTest()
        {
            repository = new UserMockData();
            userService = new UserService(repository);
        }

        [TestMethod]
        public async Task GetUsers_Default_ReturnsAllUsers()
        {
            // Arrange
            var expectedCount = 5;

            // Act
            var actualCount = (await userService.GetUsersAsync()).ToList().Count;

            // Assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public async Task AddUserAsync_AddStudentUser_IncrementCount()
        {
            // Arrange
            var expectedCount = 6;
            User user = new User("Mike0408@edu.easj.dk");
            await userService.AddUserAsync(user);
            // Act
            var actualCount = (await userService.GetUsersAsync()).ToList().Count;
            // Assert
            Assert.AreEqual(expectedCount, actualCount);

        }

        [TestMethod]
        public async Task AddUserAsync_AddUserEmailAlreadyExists_DoesNotIncrementCount()
        {
            // Arrange
            int expectedCount = 5;
            string emailInUse = "stev0408@edu.easj.dk";
            User user = new User(emailInUse);
            userService.AddUserAsync(user);
            int actualUserCount;
            // Act
            actualUserCount = (await userService.GetUsersAsync()).ToList().Count;
            // Assert
            Assert.AreEqual(expectedCount, actualUserCount);
        }
        [TestMethod]
        public async Task DeleteUserAsync_RemovesUser_DecreasesCount()
        {
            // Arrange
            var expectedCount = 4;
            var id = 1;
            await userService.DeleteUserAsync(id);
            // Act
            int actualCount = (await userService.GetUsersAsync()).ToList().Count;

            // Assert
            Assert.AreEqual(expectedCount, actualCount);

        }
        [TestMethod]
        public async Task GetUserByIdAsync_ValidId_ReturnsUserObject()
        {
            string expectedEmail = "Osca8786@gmail.com";

            // Act
            User actualUser = await userService.GetUserByIdAsync(3);

            // Assert
            //Assert.AreEqual(expectedname, actualUser.Name);
            Assert.AreEqual(expectedEmail, actualUser.Email);
        }

        [TestMethod]
        public async Task UpdateUserAsync_UpdateExsitingUser_ReturnsUpdatedObject()
        {
            // Arrange
            User user = await userService.GetUserByIdAsync(3);
            string expectedEmail = "stev0408@edu.easj.dk";

            // Act
            //user.Name = expectedName;
            user.Email = expectedEmail;
            await userService.UpdateUserAsync(user);
            User actualUser = await userService.GetUserByIdAsync(3);

            // Assert

            Assert.AreEqual(expectedEmail, actualUser.Email);
        }

        [TestMethod]
        public async Task EmailInUseAsync_ValidLogin_ReturnsTrue()
        {
            // Arrange
            string correctEmail = "stev0408@edu.easj.dk";
            bool expectedLoginResult;
            // Act
            expectedLoginResult = await userService.EmailInUseAsync(correctEmail);

            // Assert

            Assert.IsTrue(expectedLoginResult);

        }


        [TestMethod]
        public async Task EmailInUseAsync_InvalidEmailLogin_ReturnsFalse()
        {
            // Arrange
            string inCorrectEmail = "Steven@outlook.com";
            bool expectedLoginResult;
            // Act
            expectedLoginResult = await userService.EmailInUseAsync(inCorrectEmail);

            // Assert

            Assert.IsFalse(expectedLoginResult);

        }

        [TestMethod]
        public void CreateClaim_ValidEmail_ReturnsClaimIdentity()
        {
            // Arrange
            string expectedClaimName = "stev0408@edu.easj.dk";
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
            string email = "stev0408@edu.easj.dk";
            ClaimsIdentity actualClaimIdentity = userService.CreateClaimIdentity(email);
            // Act
            Claim claimRole = actualClaimIdentity.Claims.FirstOrDefault(role => role.Value == "admin");
            // Assert
            Assert.IsNull(claimRole);
        }

        internal class UserMockData : IUserDb
        {
            private static List<User> _users;
            private readonly PasswordHasher<string> passwordHasher;
            DimselabDbContext dbContext;
            public UserMockData()
            {
                passwordHasher = new PasswordHasher<string>();
                var options = new DbContextOptionsBuilder<DimselabDbContext>()
                       .UseInMemoryDatabase(Guid.NewGuid().ToString()).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                       .Options;
                dbContext = new DimselabDbContext(options);
                LoadDatabase();

                //dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            public async Task AddObjectAsync(User obj)
            {
                await dbContext.Set<User>().AddAsync(obj);
                await dbContext.SaveChangesAsync();
            }

            public async Task DeleteObjectAsync(User obj)
            {
                dbContext.Set<User>().Remove(obj);
                await dbContext.SaveChangesAsync();
            }

            public async Task<User> GetObjectByKeyAsync(int id)
            {
                return await dbContext.Set<User>().FindAsync(id);
            }

            public async Task<IEnumerable<User>> GetObjectsAsync()
            {
                return await dbContext.Set<User>().AsNoTracking().ToListAsync();
            }

            public async Task UpdateObjectAsync(User obj)
            {
                dbContext.Set<User>().Update(obj);
                await dbContext.SaveChangesAsync();

            }

            public void DropDatabase()
            {
                dbContext.Database.EnsureDeleted();
            }

            private void LoadDatabase()
            {
                dbContext.Users.Add(new User("stev0408@edu.easj.dk","student"));
                dbContext.Users.Add(new User("Mikk0908@edu.easj.dk", "student"));
                dbContext.Users.Add(new User("Osca8786@gmail.com", "student"));
                dbContext.Users.Add(new User("Chri@gmail.com", "teacher"));
                dbContext.Users.Add(new User("Admin@Dimselab.dk","admin", PasswordEncrypt("Admin1234")));

                dbContext.SaveChangesAsync();
            }

            private string PasswordEncrypt(string password)
            {
                return passwordHasher.HashPassword(null, password);
            }

            public async Task<User> GetUserByEmail(string email)
            {

                return dbContext.Users.SingleOrDefault(u => u.Email.ToLower() == email.ToLower());

            }

            public async Task<bool> DoesEmailExist(string email)
            {

                return dbContext.Users.Any(u => u.Email.ToLower() == email.ToLower());

            }
        }
    }
}
