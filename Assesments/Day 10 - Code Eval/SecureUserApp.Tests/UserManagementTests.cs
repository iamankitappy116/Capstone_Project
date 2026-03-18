using NUnit.Framework;
using SecureUserApp.Services;
using SecureUserApp.Utils;

namespace SecureUserApp.Tests
{
    [TestFixture]
    public class UserManagementTests
    {
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userService = new UserService();
        }

        // ---------------- REGISTER TESTS ----------------

        [Test]
        public void Register_WithValidDetails_ReturnsTrue()
        {
            bool result = _userService.Register("john", "Pass@123", "john@test.com");

            Assert.That(result, Is.True);
        }

        [Test]
        public void Register_WithDuplicateUsername_ReturnsFalse()
        {
            _userService.Register("john", "Pass@123", "john@test.com");

            bool result = _userService.Register("john", "AnotherPass", "other@test.com");

            Assert.That(result, Is.False);
        }

        [Test]
        public void Register_WithEmptyUsername_ReturnsFalse()
        {
            bool result = _userService.Register("", "Pass@123", "detail");

            Assert.That(result, Is.False);
        }

        [Test]
        public void Register_WithEmptyPassword_ReturnsFalse()
        {
            bool result = _userService.Register("john", "", "detail");

            Assert.That(result, Is.False);
        }

        [Test]
        public void Register_WithNullPassword_ReturnsFalse()
        {
            bool result = _userService.Register("testUser", null, "detail");

            Assert.That(result, Is.False);
        }

        // ---------------- LOGIN TESTS ----------------

        [Test]
        public void Login_WithCorrectCredentials_ReturnsTrue()
        {
            _userService.Register("jane", "Jane@789", "detail");

            bool result = _userService.Login("jane", "Jane@789");

            Assert.That(result, Is.True);
        }

        [Test]
        public void Login_WithWrongPassword_ReturnsFalse()
        {
            _userService.Register("jane", "Jane@789", "detail");

            bool result = _userService.Login("jane", "WrongPassword");

            Assert.That(result, Is.False);
        }

        [Test]
        public void Login_WithUnknownUser_ReturnsFalse()
        {
            bool result = _userService.Login("unknown", "Pass@123");

            Assert.That(result, Is.False);
        }

        [Test]
        public void Login_WithNullUsername_ReturnsFalse()
        {
            bool result = _userService.Login(null, "somePass");

            Assert.That(result, Is.False);
        }

        // ---------------- HASHING TESTS ----------------

        [Test]
        public void HashPassword_SameInputProducesSameHash()
        {
            string hash1 = HashHelper.ComputeSha256Hash("myPassword");
            string hash2 = HashHelper.ComputeSha256Hash("myPassword");

            Assert.That(hash1, Is.EqualTo(hash2));
        }

        [Test]
        public void HashPassword_DifferentInputsProduceDifferentHashes()
        {
            string hash1 = HashHelper.ComputeSha256Hash("password1");
            string hash2 = HashHelper.ComputeSha256Hash("password2");

            Assert.That(hash1, Is.Not.EqualTo(hash2));
        }

        [Test]
        public void HashPassword_IsNotSameAsPlainText()
        {
            string plain = "secret";
            string hash = HashHelper.ComputeSha256Hash(plain);

            Assert.That(hash, Is.Not.EqualTo(plain));
        }

        // ---------------- ENCRYPTION TESTS ----------------

        [Test]
        public void EncryptData_ReturnsEncryptedString()
        {
            string original = "sensitiveInfo";
            string encrypted = EncryptionHelper.EncryptData(original);

            Assert.That(encrypted, Is.Not.EqualTo(original));
        }

        [Test]
        public void DecryptData_ReturnsOriginalString()
        {
            string original = "sensitiveInfo";
            string encrypted = EncryptionHelper.EncryptData(original);
            string decrypted = EncryptionHelper.DecryptData(encrypted);

            Assert.That(decrypted, Is.EqualTo(original));
        }

        [Test]
        public void EncryptAndDecrypt_WorksForEmptyString()
        {
            string original = "";
            string encrypted = EncryptionHelper.EncryptData(original);
            string decrypted = EncryptionHelper.DecryptData(encrypted);

            Assert.That(decrypted, Is.EqualTo(original));
        }

        [Test]
        public void GetDecryptedDetails_ReturnsCorrectValue()
        {
            _userService.Register("alice", "AlicePass", "alice@example.com");

            string detail = _userService.GetDecryptedDetails("alice");

            Assert.That(detail, Is.EqualTo("alice@example.com"));
        }

        // ---------------- ERROR HANDLING ----------------

        [Test]
        public void DecryptData_InvalidInput_ThrowsException()
        {
            Assert.Throws<System.InvalidOperationException>(() =>
            {
                EncryptionHelper.DecryptData("invalid-base64");
            });
        }

        // ---------------- INTEGRATION TEST ----------------

        [Test]
        public void Register_Login_FullFlow_DoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
            {
                _userService.Register("logUser", "Log@Pass1", "log@test.com");
                _userService.Login("logUser", "Log@Pass1");
                _userService.GetDecryptedDetails("logUser");
            });
        }
    }
}