using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPCoreWebAPICRUD.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string StudentName { get; set; }

        [Required]
        [StringLength(10)]
        public string StudentGender { get; set; }

        public int Age { get; set; }

        public int Standard { get; set; }

        [StringLength(100)]
        public string? FatherName { get; set; }   // optional field
    }
}
