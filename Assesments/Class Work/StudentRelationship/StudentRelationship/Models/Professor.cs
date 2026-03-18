namespace StudentRelationship.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int StreamId { get; set; }
        public StudentStream Stream { get; set; }
    }
}
