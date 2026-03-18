namespace StudentRelationship.Models
{
    public class StudentStream
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }

        public List<Student> Students { get; set; }
        public List<Professor> Professors { get; set; }
    }
}
