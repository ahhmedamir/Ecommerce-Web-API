﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
   public record BasketDto
    {
        public string Id { get; init; }
        public  IEnumerable<BasketItemDto> Items { get; init; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }

    }
}
