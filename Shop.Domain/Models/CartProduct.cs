using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Domain.Models
{
    public class CartProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float Value { get; set; }
        public int StockId { get; set; }
        public int Qty { get; set; }
    }
}
