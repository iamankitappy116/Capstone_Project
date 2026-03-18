namespace AtomicityExSalesProduct.Models
{
    public class OrderedItem
    {
        public int Id { get; set; }

        // Foreign key to Product
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        // Foreign key to AppUser
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;

        // Timestamp of when the order was placed
        public DateTime OrderedAt { get; set; }
    }
}
