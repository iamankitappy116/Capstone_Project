using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Medicine
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Pharma")]
        public int PharmacyId { get; set; }

        public Pharmacy? Pharma { get; set; }

        public int Qta { get; set; }
    }
}
