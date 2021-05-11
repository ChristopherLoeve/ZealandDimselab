using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZealandDimselab.Models;

namespace ZealandDimselab.Services
{
    public class UserService: GenericService<User>
    {
        private PasswordHasher<string> _passwordHasher;

        public UserService(IDbService<User> DbService): base(DbService)
        {
            _passwordHasher = new PasswordHasher<string>();
            // Test user: Admin Admin@Dimselab.dk secret1234 (don't tell anyone)
            // Test user: Oscar Oscar@email.com password
            //User user = new User("Oscar", "Oscar@email.com", "password");
            //AddUserAsync(user);
        }

        public List<User> GetUsersAsync()
        {
            return GetAllObjects();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await GetObjectByKeyAsync(id);
        }

        public User GetUserByEmail(string email)
        {
            return GetUsersAsync().SingleOrDefault(u => u.Email.ToLower() == email.ToLower()); // Checks all users in list "users" if incoming email matches one of them.
        }

        public async Task AddUserAsync(User user)
        {
            user.Password = _passwordHasher.HashPassword(null, user.Password);
            await AddObjectAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await DeleteObjectAsync(await GetUserByIdAsync(id));
        }
        
        public async Task UpdateUserAsync(User user)
        {
            await UpdateObjectAsync(user);
        }

        public bool ValidateLogin(string email, string providedPassword)
        {
            var user = GetUserByEmail(email);
            if (user != null)
            {
                if (PasswordVerification(user.Password, providedPassword) == PasswordVerificationResult.Success) // Checks if password matches password.
                {
                    return true;
                }
            }
            return false;
        }
        private PasswordVerificationResult PasswordVerification(string hashedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        }

        public ClaimsIdentity CreateClaimIdentity(string email)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email)
            };

            if (email.ToLower() == "Admin@Dimselab.dk".ToLower()) // This checks if the user attempts to login as an administrator account.
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return claimsIdentity;
        }
        


    }
}
