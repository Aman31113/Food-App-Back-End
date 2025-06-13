using Microsoft.EntityFrameworkCore;// Imports tools from Entity Framework Core, which helps you work with databases in C#.
using Online_food_delivery_system.Interface; // Imports the 'IUser' blueprint (interface) we discussed earlier. This class will follow that blueprint.
using Online_food_delivery_system.Models;// Imports the 'User', 'Customer', 'Agent', and 'Restaurant' definitions (models) used in your application.

namespace Online_food_delivery_system.Repository
{
    public class UserRepository : IUser// The ': IUser' part means this class promises to fulfill all the methods defined in the 'IUser' interface (our blueprint).
    {
        private readonly FoodDbContext _context; // Declares a private field named '_context'. This will hold the connection to your database.
                                                 // 'readonly' means it can only be set when the object is created.
        public UserRepository(FoodDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // It asynchronously fetches all 'User' records from the database and returns them as a list.
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetByIdAsync(int userId)
        {
            // 'FirstOrDefaultAsync' returns the first matching user or 'null' if no user is found.
            return await _context.Users.FirstOrDefaultAsync(u => u.ID == userId);
        }
        public async Task AddAsync(User user)
        {
            // First, check if a user with the same email already exists in the Users table.
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists");
            }
            // Check if the user's role is "customer" (case-insensitive).
            if (user.Role?.ToLower() == "customer")
            {
                // Check if a customer with this email already exists in the Customers table.
                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == user.Email);
                if (existingCustomer == null)
                {
                    var customer = new Customer
                    {
                        Name = user.Username,
                        Email = user.Email,
                        Phone = "+91",  //Set a default
                        Address = "Address" //Set a default

                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                }
            }
            else if (user.Role?.ToLower() == "agent")
            {
                var existingCustomer = await _context.Agents.FirstOrDefaultAsync(c => c.Email == user.Email);
                if (existingCustomer == null)
                {
                    var agent = new Agent
                    {
                        Name = user.Username,
                        Email = user.Email,
                        AgentContact = "+91"

                    };
                    _context.Agents.Add(agent);
                    await _context.SaveChangesAsync();
                }
            }
            else if (user.Role?.ToLower() == "restaurant")
            {
                var existingCustomer = await _context.Restaurants.FirstOrDefaultAsync(c => c.Email == user.Email);
                if (existingCustomer == null)
                {
                    var restaurant = new Restaurant
                    {
                        RestaurantName = user.Username,
                        Email = user.Email,
                        RestaurantContact = "+91",
                        Address = "default",
                        Availability = true, 

                    };
                    _context.Restaurants.Add(restaurant);
                    await _context.SaveChangesAsync();
                }
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User user)
        {

            // Tell the database context that this 'user' object has been modified.
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId)
        {
            // First, try to find the user in the database by their ID.
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user); // Tell the database context to remove this user.
                await _context.SaveChangesAsync();
            }
        }
    }



}
  