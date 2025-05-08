using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.OrderModels;


namespace Services.Abstractions
{
    public interface IOrderService
    {
        //Get Order By Id  =>
        public Task<OrderResult> GetOrderByIdAsync(Guid id);

        // Get Orders For User By Email =>
        public Task<IEnumerable<OrderResult>> GetOrderByEmailAsync(string email);


        //Create Order =>
        public Task<OrderResult> CreateOrUpdateOrderAsync(OrderRequest request, string userEmail);

        //Get All Delivey Methods=>
        public Task<IEnumerable<DeliverMethodResult>> GetDeliveryMethodsAsync();

    }
}
