using System.Security.Cryptography;

namespace AspNetMvcDemo.Services;

/**
 * Provides password hashing functionality for the application.
 * This class is used to securely hash user passwords before storing them in the database.
 */
public static class PasswordService
{
    /**
     * Hashes a password using PBKDF2 with SHA256.
     * @param password The password to hash.
     * @return The hashed password.
     */
    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            100_000,
            HashAlgorithmName.SHA256,
            32);

        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    /**
     * Verifies a password against a stored hash.
     * @param password The password to verify.
     * @param storedPassword The stored password.
     * @return True if the password is correct, false otherwise.
     */
    public static bool VerifyPassword(
        string password,
        string storedPassword)
    {
        string[] parts = storedPassword.Split('.');

        if (parts.Length != 2)
        {
            return false;
        }

        // Extract the salt and hash from the stored password
        byte[] salt = Convert.FromBase64String(parts[0]);
        // Extract the expected hash from the stored password
        byte[] expectedHash = Convert.FromBase64String(parts[1]);

        byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            100_000,
            HashAlgorithmName.SHA256,
            32);
        // Use a constant-time comparison
        // can replace with return actualHash.SequenceEqual(expectedHash);
        return CryptographicOperations.FixedTimeEquals(
            actualHash,
            expectedHash);
    }
}