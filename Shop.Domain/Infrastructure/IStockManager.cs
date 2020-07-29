﻿using Shop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
        public interface IStockManager
        {
            Task<int> CreateStock(Stock stock);
            Task<int> UpdateStockRange(List<Stock> stockList);
            Task<int> DeleteStock(int id);

            Stock GetStockWithProduct(int stockId);
            bool EnoughStock(int stockId, int qty);
            Task PutStockOnHold(int stockId, int qty, string sessionId);
            
            Task RetrieveExpiredStockOnHold();
            Task RemoveStockFromHold(string sessionId);
            Task RemoveStockFromHold(int stockId, int qty, string sessionId);
        }
}