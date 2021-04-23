﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZealandDimselab.Models;
using ZealandDimselab.Repository;

namespace ZealandDimselab.Services
{
    public class UserService
    {
        private readonly IRepository<User> repository;
        private readonly List<User> _users;

        public UserService(IRepository<User> repository)
        {
            this.repository = repository;
            _users = repository.GetAllAsync();
        }


    }
}