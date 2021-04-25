﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZealandDimselab.Models;
using ZealandDimselab.Repository;

namespace ZealandDimselab.Services
{
    public class UserService
    {
        private readonly IRepository<User> repository;
        private Dictionary<int, User> _users;

        public UserService(IRepository<User> repository)
        {
            this.repository = repository;
            _users = repository.GetAllAsync().ToDictionary(u => u.Id);
        }

        public List<User> GetUsers()
        {
            return _users.Values.ToList();
        }
        /// <summary>
        /// Adds an user object.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddUserAsync(User user)
        {
            _users.Add(user.Id, user);
            await repository.AddObjectAsync(user);

        }
        /// <summary>
        /// Delete an user object.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteUserAsync(int id)
        {
            await repository.DeleteObjectAsync(_users[id]);
            _users.Remove(id);
        }

        /// <summary>
        /// Updates an user object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateUserAsync(int id, User user)
        {
            if (user != null)
            {
                await repository.UpdateObjectAsync(user);
                _users = repository.GetAllAsync().ToDictionary(i => i.Id);
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

            if (email == "Admin@Dimselab") // This checks if the user attempts to login as an administrator account.
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return claimsIdentity;
        }

        // Gets an user by email.
        private User GetUserByEmail(string email)
        {
            return _users.Values.SingleOrDefault(u => u.Email == email); // Checks all users in list "users" if incoming email matches one of them.
        }

        // Verifies the provided password.
        private PasswordVerificationResult PasswordVerification(string hashedPassword, string providedPassword)
        {
            var passwordHasher = new PasswordHasher<string>();

            return passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        }

    }
}
