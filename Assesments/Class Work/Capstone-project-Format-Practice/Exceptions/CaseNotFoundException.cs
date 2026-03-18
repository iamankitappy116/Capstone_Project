namespace Crime.Exceptions
{
    public class CaseNotFoundException : Exception
    {
        public CaseNotFoundException(string message) : base(message) { }
    }
}
