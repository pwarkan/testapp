using Shop.Domain.Enums;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IOrderManager
    {
        bool OrderReferenceExists(string reference);

        Task<int> AdvanceOrder(int id);

        IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Func<Order, TResult> selector);
        TResult GetOrderById<TResult>(int id, Func<Order, TResult> selector);
        TResult GetOrderByReference<TResult>(string reference, Func<Order, TResult> selector);
        Task<int> CreateOrder(Order order);
    }
}
