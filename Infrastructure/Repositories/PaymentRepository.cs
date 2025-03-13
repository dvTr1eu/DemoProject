using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PaymentRepository(DemoDbContext dbContext) : IPaymentRepository
    {
        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            var results = await dbContext.Payments
                .Include(p => p.Booking)
                .ToListAsync();
            return results;
        }

        public async Task<Payment> FindByCondition(Expression<Func<Payment, bool>> condition, string[]? includes)
        {
            var dataset = dbContext.Payments.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    dataset = dataset.Include(include);
                }
            }

            return await dataset.FirstOrDefaultAsync(condition);
        }

        public async Task AddAsync(Payment entity)
        {
            await dbContext.Payments.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
