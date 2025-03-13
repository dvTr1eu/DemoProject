using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Core.Entities;
using Core.Repositories;

namespace Infrastructure.Services
{
    public class PaymentService(IPaymentRepository paymentRepository) : IPaymentService
    {
        public async Task<IEnumerable<Payment>> GetAllPayment()
        {
            var results = await paymentRepository.GetAllAsync();
            return results;
        }

        public async Task<bool> CreatePayment(Payment model)
        {
            try
            { 
                await paymentRepository.AddAsync(model);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
