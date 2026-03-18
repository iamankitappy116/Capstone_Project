namespace Crime.Exceptions
{
    public class IncidentNotFoundException : Exception
    {
        public IncidentNotFoundException(string message) : base(message) { }
    }
}
