﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; }
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool Status { get; set; }
    }
}
