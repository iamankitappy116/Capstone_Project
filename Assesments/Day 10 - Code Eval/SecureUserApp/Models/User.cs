namespace SecureUserApp.Models
{
    public class User
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string EncryptedDetails { get; set; }
    }
}