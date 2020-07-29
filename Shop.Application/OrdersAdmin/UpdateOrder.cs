﻿
using Shop.Domain.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.OrdersAdmin
{
    [Service]
    public class UpdateOrder
    {
        private IOrderManager _orderManager;

        public UpdateOrder(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public Task<int> Do(int id)
        {
            return _orderManager.AdvanceOrder(id);
        }
    }
}
