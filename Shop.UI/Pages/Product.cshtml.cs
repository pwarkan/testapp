using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Application.Products;
using Shop.Database;

namespace Shop.UI.Pages
{
    public class ProductModel : PageModel
    {
        private AppDbContext _ctx;

        public ProductModel(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [BindProperty]
        public AddToCart.Request CartViewModel { get; set; }

        public GetProduct.ProductViewModel  Product { get; set; }

        public async Task<IActionResult> OnGet(
            string name,
            [FromServices] GetProduct getProduct)
        {
            Product = await getProduct.Do(name.Replace("-", " "));
            if (Product == null)
                return RedirectToPage("Index");
            else
                return Page();
        }

        public async Task<IActionResult> OnPost(
            [FromServices] AddToCart addToCart)
        {
           var stockAdded = await addToCart.Do(CartViewModel);

            if (stockAdded)
                return RedirectToPage("Cart");
            else
                return Page();
        }
    }
}
