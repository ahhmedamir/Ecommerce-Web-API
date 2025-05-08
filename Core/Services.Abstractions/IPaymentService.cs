﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Services.Abstractions
{
    public interface IPaymentService
    {
        public Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId);
        public Task UpdateOrderPaymentStatus(string request, string stripHeader);
    }
}
