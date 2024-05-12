﻿using BookShopApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repository
{
    public class OrderRepository:IOrderRepository
    {
        private readonly BookShopContext _context;

        public OrderRepository(BookShopContext context)
        {
            _context = context;
        }
        public async Task Add(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Order>> GetAll()
        {
            //return await _context.Orders.Include(x=>x.Book).ToListAsync();
            throw new NotImplementedException();
        }
        public async Task<Order> GetById(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(x=>x.Id == id);
        }
        public async Task<IEnumerable<Order>> GetUserOrders(int userId)
        {
            //return await _context.Orders.Where(x=>x.UserId== userId).Include(x=>x.Book)
            //    .ThenInclude(x=>x.Categories).ThenInclude(x=>x.Category).ToListAsync();
            throw new NotImplementedException();
        }
    }
}
