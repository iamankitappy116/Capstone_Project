namespace AtomicityExSalesProduct.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        // Foreign key to Product
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        // Foreign key to AppUser
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;
    }
}
