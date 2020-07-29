using Microsoft.EntityFrameworkCore;
using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Database
{
    public class OrderManager : IOrderManager
    {
        private readonly AppDbContext _ctx;

        public OrderManager(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public bool OrderReferenceExists(string reference)
        {
            return _ctx.Orders.Any(x => x.OrderRef == reference);
        }

        private TResult GetOrder<TResult>(
            Func<Order, bool> condition,
            Func<Order, TResult> selector)
        {
            return _ctx.Orders
               .Where(x => condition(x))
               .Include(x => x.OrderStocks)
                   .ThenInclude(x => x.Stock)
                       .ThenInclude(x => x.Product)
               .Select(selector)
               .FirstOrDefault();
        }

        public TResult GetOrderById<TResult>(int id, Func<Order, TResult> selector)
        {
            return GetOrder( order => order.Id == id, selector);
        }

        public TResult GetOrderByReference<TResult>(
            string reference, 
            Func<Order, TResult> selector)
        {
            return GetOrder(x => x.OrderRef == reference, selector);
        }

        public Task<int> CreateOrder(Order order)
        {

            _ctx.Orders.Add(order);

            return _ctx.SaveChangesAsync();
        }

        public IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Func<Order, TResult> selector)
        {
           return _ctx.Orders.AsEnumerable()
               .Where(x => x.Status == status)
           .Select(selector)
           .ToList();
        }

        public Task<int> AdvanceOrder(int id)
        {
            _ctx.Orders.FirstOrDefault(x => x.Id == id).Status++;

            return _ctx.SaveChangesAsync();
        }
    }
}
