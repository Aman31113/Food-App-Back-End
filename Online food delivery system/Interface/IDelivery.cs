﻿using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Interface
{
    public interface IDelivery
    {
        Task<IEnumerable<Delivery>> GetAllAsync();
        Task<Delivery> GetByIdAsync(int deliveryId);
        Task AddAsync(Delivery delivery);
        Task UpdateAsync(Delivery delivery);
        Task DeleteAsync(int deliveryId);
    }
}
