using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Interface // This creates a special labeled box called 'Interface'for our code.
//It helps keep things tidy so your "user blueprints" don't get mixed up with other nparts of your app.

{
    public interface IUser
    {
        // The 'Async' part means this task might take a little time (like fetching from a big list of users),
        // so the app should be able to do other things while it waits.
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}


