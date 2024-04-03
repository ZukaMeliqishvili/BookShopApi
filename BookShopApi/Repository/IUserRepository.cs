﻿using BookShopApi.Entities;

namespace BookShopApi.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUser(string userName);
        Task Insert(User category);
        Task<bool> Exists(string Username);
    }
}