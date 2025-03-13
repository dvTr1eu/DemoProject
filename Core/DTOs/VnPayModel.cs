using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class VnPayModel
    {
        public class VnPayResponseVM
        {
            public bool Success { get; set; }
            public string PaymentMethod { get; set; }
            public string OrderDescription { get; set; }
            public string BookId { get; set; }
            public string PaymentId { get; set; }
            public string TransactionId { get; set; }
            public string Token { get; set; }
            public string VnPayResponseCode { get; set; }
        }

        public class VnPaymentRequestVM
        {
            //public string FullName { get; set; }
            public int BookId { get; set; }
            public string Description { get; set; }
            public double Amount { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}
