﻿using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Repository
{
    public class MenuItemRepository: IMenuItem
    {
            private readonly FoodDbContext _context;

            public MenuItemRepository(FoodDbContext context)
            {
                _context = context;
            }

            public async Task AddAsync(MenuItem menuItem)
            {
                await _context.MenuItems.AddAsync(menuItem);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int itemId)
            {
                var menuItem = await _context.MenuItems.FindAsync(itemId);
                if (menuItem != null)
                {
                    _context.MenuItems.Remove(menuItem);
                    await _context.SaveChangesAsync();
                }
            }

            public async Task<IEnumerable<MenuItem>> GetAllAsync()
            {
                return await _context.MenuItems.ToListAsync();
            }

            public async Task<MenuItem> GetByIdAsync(int itemId)
            {
                return await _context.MenuItems.FindAsync(itemId);
            }

            public async Task UpdateAsync(MenuItem menuItem)
            {
                _context.MenuItems.Update(menuItem);
                await _context.SaveChangesAsync();
            }
        
    }


}
