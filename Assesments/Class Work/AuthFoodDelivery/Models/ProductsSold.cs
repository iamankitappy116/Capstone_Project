using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class ProductsSold
    {
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Food Food { get; set; }


        public int SaleId { get; set; }

        [ForeignKey("SaleId")]
        public Sale Sale { get; set; }


        public int Qty { get; set; }

        public decimal TotalProductAmount { get; set; }

        public string Status { get; set; }
    }
}