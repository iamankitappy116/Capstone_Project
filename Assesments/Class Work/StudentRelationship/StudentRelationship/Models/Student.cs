namespace StudentRelationship.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int StreamId { get; set; }
        public StudentStream Stream { get; set; }

        public int ParentId { get; set; }
        public Parent Parent { get; set; }
    }
}
