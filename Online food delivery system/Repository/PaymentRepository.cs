using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Repository
{
    public class PaymentRepository : IPayment
    {
        private readonly FoodDbContext _context;
        public PaymentRepository(FoodDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int PaymentID)
        {
            var pay = await _context.Payments.FindAsync(PaymentID);
            if (pay != null)
            {
                _context.Payments.Remove(pay);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.Include(p => p.Order)
                .ThenInclude(o => o.Customer)
                .Include(p => p.Order)
                .ThenInclude(o => o.Restaurant)
                //.Include(p => p.Delivery)
                //.ThenInclude(p=>p.Agent)
                .ToListAsync();

        }

        public async Task<Payment> GetByIdAsync(int ID)
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Customer)
                .Include(p=>p.Order)
                .ThenInclude(o=>o.Restaurant)
                .FirstOrDefaultAsync(p => p.PaymentID == ID);

            if (payment == null)
            {
                throw new Exception("Payment not found");
            }
            return payment;
        }

        public async Task<Payment> ProcessPaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
