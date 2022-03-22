using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZealandDimselab.Interfaces;
using ZealandDimselab.Models;

namespace ZealandDimselab.Services
{
    public class UserDbService : GenericDbService<User>, IUserDb
    {
        //public async Task<bool> DoesEmailExist(string email)
        //{
        //    await using (var context = new DimselabDbContext())
        //    {
        //        return context.Users.Any(u => u.Email.ToLower() == email.ToLower());
        //    }
        //}

        //public async Task<User> GetUserByEmail(string email)
        //{
        //    await using (var context = new DimselabDbContext())
        //    {
        //        return context.Users.SingleOrDefault(u => u.Email.ToLower() == email.ToLower());
        //    }
        //}
        public Task<bool> DoesEmailExist(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
