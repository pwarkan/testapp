﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Shop.Database;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Database
{
        public class StockManager : IStockManager
        {
            private AppDbContext _ctx;

            public StockManager(AppDbContext ctx)
            {
                _ctx = ctx;
            }

        public Task<int> CreateStock(Stock stock)
        {
            _ctx.Stock.Add(stock);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> DeleteStock(int id)
        {
            var stock = _ctx.Stock.FirstOrDefault(x => x.Id == id);

            _ctx.Stock.Remove(stock);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> UpdateStockRange(List<Stock> stockList)
        {
            _ctx.Stock.UpdateRange(stockList);

            return _ctx.SaveChangesAsync();
        }

        public bool EnoughStock(int stockId, int qty)
            {
                return _ctx.Stock.FirstOrDefault(x => x.Id == stockId).Qty >= qty;
            }

            public Stock GetStockWithProduct(int stockId)
            {
                return _ctx.Stock
                    .Include(x => x.Product)
                    .FirstOrDefault(x => x.Id == stockId);
            }

            public Task PutStockOnHold(int stockId, int qty, string sessionId)
            {

                _ctx.Stock.FirstOrDefault(x => x.Id == stockId).Qty -= qty;

                var stockOnHold = _ctx.StocksOnHold
                    .Where(x => x.SessionId == sessionId)
                    .ToList();

                if (stockOnHold.Any(x => x.StockId == stockId))
                {
                    stockOnHold.Find(x => x.StockId == stockId).Qty += qty;
                }
                else
                {
                    _ctx.StocksOnHold.Add(new StockOnHold
                    {
                        StockId = stockId,
                        SessionId = sessionId,
                        Qty = qty,
                        ExpiryDate = DateTime.Now.AddMinutes(20)
                    });
                }

                foreach (var stock in stockOnHold)
                {
                    stock.ExpiryDate = DateTime.Now.AddMinutes(20);
                }

                return _ctx.SaveChangesAsync();
            }



        public Task RemoveStockFromHold(string sessionId)
        {

            var stockOnHold = _ctx.StocksOnHold.AsEnumerable()
                .Where(x => x.SessionId == sessionId)
                .ToList();

            _ctx.StocksOnHold.RemoveRange(stockOnHold);

            return _ctx.SaveChangesAsync();
        }

        public Task RemoveStockFromHold(int stockId, int qty, string sessionId)
            {
                var stockOnHold = _ctx.StocksOnHold
                    .FirstOrDefault(x => x.StockId == stockId
                    && x.SessionId == sessionId);

                var stock = _ctx.Stock.FirstOrDefault(x => x.Id == stockId);
                stock.Qty += qty;
                stockOnHold.Qty -= qty;

                if (stockOnHold.Qty <= 0)
                {
                    _ctx.Remove(stockOnHold);
                }

                return _ctx.SaveChangesAsync();
            }

        public Task RetrieveExpiredStockOnHold()
        {
            var stocksOnHold = _ctx.StocksOnHold.AsEnumerable()
                .Where(x => x.ExpiryDate < DateTime.Now).ToList();

            if (stocksOnHold.Count > 0)
            {
                var stockToReturn = _ctx.Stock.AsEnumerable()
                    .Where(x => stocksOnHold.Any(y => y.StockId == x.Id)).ToList();

                foreach (var stock in stockToReturn)
                {
                    stock.Qty = stock.Qty + stocksOnHold.FirstOrDefault(x => x.StockId == stock.Id).Qty;
                }

                _ctx.StocksOnHold.RemoveRange(stocksOnHold);

                return _ctx.SaveChangesAsync();
            }

            return Task.CompletedTask;
        }

    }
    }
