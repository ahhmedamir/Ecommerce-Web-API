using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Shared;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _LazyBasketService;
        private readonly Lazy<IAuthenticationService> _AuthenticationService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IPaymentService> _paymentService;
        private readonly Lazy<ICashService> _cashService;
        public ServiceManager(IUnitOfWork unitOfWork,IMapper mapper,IBasketRepository basketRepository,UserManager<User> userManager,IConfiguration configuration , IOptions<JwtOptions> options,ICashRepository cashRepository)
        {
            _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
            _LazyBasketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
            _AuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, configuration,options,mapper));
            _orderService = new Lazy<IOrderService>(() => new OrderService(mapper, unitOfWork, basketRepository));
            _paymentService = new Lazy<IPaymentService>(() => new PaymentService(basketRepository, mapper, configuration, unitOfWork));
            _cashService = new Lazy<ICashService>(() => new CasheService(cashRepository));
        
        
        }
        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _LazyBasketService.Value;

        public IAuthenticationService AuthenticationService => _AuthenticationService.Value;
        public IOrderService OrderService => _orderService.Value;

        public IPaymentService PaymentService => _paymentService.Value;

        public ICashService CashService => _cashService.Value;
    }
}
