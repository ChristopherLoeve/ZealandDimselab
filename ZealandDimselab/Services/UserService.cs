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
    public class UserService
    {
        private readonly IDbService<User> DbContext;
        private List<User> _users;
        private PasswordHasher<string> passwordHasher;

        public UserService(IDbService<User> DbContext)
        {
            this.DbContext = DbContext;
            _users = DbContext.GetObjectsAsync().Result.ToList();

        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await DbContext.GetObjectsAsync();
        }
        /// <summary>
        /// Adds an user object.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddUserAsync(User user)
        {
            passwordHasher = new PasswordHasher<string>();
            user.Password = passwordHasher.HashPassword(null, user.Password);
            _users.Add(user);
            await DbContext.AddObjectAsync(user);

        }
        /// <summary>
        /// Delete an user object.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteUserAsync(int id)
        {
            User user = await GetUserByIdAsync(id);
            await DbContext.DeleteObjectAsync(user);
            _users.Remove(user);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await DbContext.GetObjectByIdAsync(id);
        }

        /// <summary>
        /// Updates an user object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateUserAsync(User user)
        {
            if (user != null)
            {
                await DbContext.UpdateObjectAsync(user);
                _users = GetUsersAsync().Result.ToList();
            }
        }

        /// <summary>
        /// Validates an user email.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidateLogin(string email, string password)
        {
            User user = GetUserByEmail(email);
            if (user != null)
            {
                if (PasswordVerification(user.Password, password) == PasswordVerificationResult.Success) // Checks if password matches password.
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// Creates an ClaimsIdentity based on the provided email.
        /// </summary>
        /// <param name="email">Email of the user.</param>
        /// <returns>ClaimsIdentity</returns>
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

        // Gets an user by email.
        private User GetUserByEmail(string email)
        {
            return _users.SingleOrDefault(u => u.Email.ToLower() == email.ToLower()); // Checks all users in list "users" if incoming email matches one of them.
        }

        // Verifies the provided password.
        private PasswordVerificationResult PasswordVerification(string hashedPassword, string providedPassword)
        {
            passwordHasher = new PasswordHasher<string>();

            return passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        }

    }
}
