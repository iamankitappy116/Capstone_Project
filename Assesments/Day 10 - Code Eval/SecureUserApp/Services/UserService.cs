using SecureUserApp.Models;
using SecureUserApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace SecureUserApp.Services
{
    public class UserService
    {
        private readonly List<User> _users = new();

        // ---------------- REGISTER ----------------
        public bool Register(string username, string password, string details = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password))
                    return false;

                if (_users.Exists(u => u.Username == username))
                    return false;

                string hashedPassword =
                    HashHelper.ComputeSha256Hash(password);

                string encryptedDetails =
                    EncryptionHelper.EncryptData(details ?? "");

                _users.Add(new User
                {
                    Username = username,
                    HashedPassword = hashedPassword,
                    EncryptedDetails = encryptedDetails
                });

                Log("User registered successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Log($"Registration error: {ex.Message}");
                return false;
            }
        }

        // ---------------- LOGIN ----------------
        public bool Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password))
                    return false;

                string hashedPassword =
                    HashHelper.ComputeSha256Hash(password);

                var user = _users.Find(u =>
                    u.Username == username &&
                    u.HashedPassword == hashedPassword);

                Log("Login attempt.");

                return user != null;
            }
            catch (Exception ex)
            {
                Log($"Authentication error: {ex.Message}");
                return false;
            }
        }

        // ---------------- DECRYPT DETAILS ----------------
        public string GetDecryptedDetails(string username)
        {
            var user = _users.Find(u => u.Username == username);

            if (user == null)
                return null;

            return EncryptionHelper.DecryptData(user.EncryptedDetails);
        }

        // ---------------- LOGGING ----------------
        private void Log(string message)
        {
            File.AppendAllText("app.log",
                $"{DateTime.Now}: {message}{Environment.NewLine}");
        }

        internal bool Authenticate(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}