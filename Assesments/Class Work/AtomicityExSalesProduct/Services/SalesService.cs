using AtomicityExSalesProduct.Data;
using AtomicityExSalesProduct.Models;

namespace AtomicityExSalesProduct.Services
{
    public class SalesService
    {
        private readonly ApplicationDbContext _db;

        public SalesService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AtomicCheckout(int cartItemId)
        {
            // Open a transaction — this is the key to ATOMICITY
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                // Fetch the cart item (include Product info for the order)
                var cartItem = await _db.CartItems
                    .FindAsync(cartItemId);

                if (cartItem == null)
                    return false;

                // STEP 1 — Delete the item from the Cart table
              
                _db.CartItems.Remove(cartItem);
                await _db.SaveChangesAsync();

              
                // STEP 2 — Insert a record into the OrderedItems table

                var orderedItem = new OrderedItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity  = cartItem.Quantity,
                    AppUserId = cartItem.AppUserId,
                    OrderedAt = DateTime.Now
                };

                _db.OrderedItems.Add(orderedItem);
                await _db.SaveChangesAsync();

                // Both steps succeeded — commit the transaction
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                // Something went wrong — roll EVERYTHING back
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
