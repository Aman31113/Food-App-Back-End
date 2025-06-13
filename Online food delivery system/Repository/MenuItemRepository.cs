using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Repository
{
    public class MenuItemRepository : IMenuItem
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
        public async Task<IEnumerable<MenuItem>> GetMenuItemsByNameAsync(string menuItemName)
        {
            return await _context.MenuItems.Include(m=>m.Restaurant)
                .Where(m => m.Name != null && m.Name.Contains(menuItemName))
                // Search by name
                .ToListAsync();
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
            return await _context.MenuItems.Include(a => a.Restaurant).ToListAsync();
        }

        public async Task<MenuItem> GetByIdAsync(int itemId)
        {
            return await _context.MenuItems.Include(a => a.Restaurant).FirstOrDefaultAsync(c => c.ItemID == itemId);
        }

        public async Task UpdateAsync(MenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Restaurant>> GetRestaurantsByMenuItemNameAsync(string menuItemName)
        {
            return await _context.MenuItems
                .Where(m => m.Name != null && m.Name.Contains(menuItemName)) // Added null check for Name
                .Select(m => m.Restaurant!)
                .Include(a => a.RestaurantName)
                .Distinct()
                .ToListAsync();
        }


    }


}
