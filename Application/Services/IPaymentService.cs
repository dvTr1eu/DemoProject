﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Application.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPayment();
        Task<bool> CreatePayment(Payment model);
    }
}
