using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Shop.Application.OrdersAdmin;
using Shop.Database;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "Manager")]
    public class OrdersController : Controller
    {

        [HttpGet("")]
        public IActionResult GetOrders(
            int status,
            [FromServices] GetOrders getOrders) =>
                Ok(getOrders.Do(status));

        [HttpGet("{id}")]
        public IActionResult GetOrder(
            int id,
            [FromServices] GetOrder getOrder) =>
                Ok(getOrder.Do(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(
            int id,
            [FromServices] UpdateOrder updateOrder)
        {
            var success = await updateOrder.Do(id) > 0;

            if (success)
            {
             return Ok();
            }

            else
            {
                return BadRequest();
            }
        }
                
    }
}
