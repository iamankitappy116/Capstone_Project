using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }

        public int DeliveryPersonId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [ForeignKey("DeliveryPersonId")]
        public DeliveryPerson DeliveryPerson { get; set; }
    }
}
