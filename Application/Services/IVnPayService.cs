using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;

namespace Application.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPayModel.VnPaymentRequestVM model);
        VnPayModel.VnPayResponseVM PaymentExecute(IQueryCollection collection);
    }
}
