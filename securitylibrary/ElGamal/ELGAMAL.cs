using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    /// <summary>
    /// ElGamal public-key cryptosystem implementation
    /// A probabilistic encryption algorithm that provides semantic security
    /// </summary>
    public class ElGamal
    {
        /// <summary>
        /// Encrypts a message using ElGamal encryption
        /// </summary>
        /// <param name="primeModulus">q - Large prime modulus (security parameter)</param>
        /// <param name="generator">alpha - Generator of the multiplicative group Z*q</param>
        /// <param name="publicKey">y - Bob's public key (y = alpha^x mod q)</param>
        /// <param name="randomValue">k - Random value chosen by Alice (1 < k < q-1)</param>
        /// <param name="plaintext">m - Message to encrypt (must be < q)</param>
        /// <returns>Ciphertext pair (c1, c2) where c1 = alpha^k mod q and c2 = m * y^k mod q</returns>
        public List<long> Encrypt(int primeModulus, int generator, int publicKey, int randomValue, int plaintext)
        {
            // Validate input parameters
            if (primeModulus <= 1)
                throw new ArgumentException("Prime modulus must be greater than 1", nameof(primeModulus));
            if (plaintext >= primeModulus)
                throw new ArgumentException("Message must be smaller than the prime modulus", nameof(plaintext));
            if (randomValue <= 0 || randomValue >= primeModulus)
                throw new ArgumentException("Random value k must be between 1 and q-1", nameof(randomValue));

            // Calculate c1 = alpha^k mod q
            // This is the ephemeral key that will be sent with the ciphertext
            long ephemeralKey = ComputeModularExponentiation(generator, randomValue, primeModulus);

            // Calculate c2 = m * y^k mod q
            // This is the encrypted message
            long sharedSecret = ComputeModularExponentiation(publicKey, randomValue, primeModulus);
            long encryptedMessage = (plaintext * sharedSecret) % primeModulus;

            // Return the ciphertext as a pair (c1, c2)
            return new List<long> { ephemeralKey, encryptedMessage };
        }

        /// <summary>
        /// Decrypts a ciphertext using ElGamal decryption
        /// </summary>
        /// <param name="ephemeralKey">c1 - First part of ciphertext (alpha^k mod q)</param>
        /// <param name="encryptedMessage">c2 - Second part of ciphertext (m * y^k mod q)</param>
        /// <param name="privateKey">x - Bob's private key</param>
        /// <param name="primeModulus">q - Large prime modulus</param>
        /// <returns>The decrypted message m</returns>
        public int Decrypt(int ephemeralKey, int encryptedMessage, int privateKey, int primeModulus)
        {
            // Validate input parameters
            if (primeModulus <= 1)
                throw new ArgumentException("Prime modulus must be greater than 1", nameof(primeModulus));
            if (privateKey <= 0 || privateKey >= primeModulus)
                throw new ArgumentException("Private key must be between 1 and q-1", nameof(privateKey));

            // Calculate s = c1^x mod q
            // This computes the shared secret (alpha^k)^x = alpha^(kx) = y^k mod q
            long sharedSecret = ComputeModularExponentiation(ephemeralKey, privateKey, primeModulus);

            // Calculate s^(-1) mod q (modular inverse of shared secret)
            long sharedSecretInverse = ComputeModularInverse(sharedSecret, primeModulus);

            // Calculate m = c2 * s^(-1) mod q
            // This recovers the original message: c2 * s^(-1) = (m * s) * s^(-1) = m mod q
            long decryptedMessage = (encryptedMessage * sharedSecretInverse) % primeModulus;

            return (int)decryptedMessage;
        }

        /// <summary>
        /// Computes modular exponentiation efficiently using the square-and-multiply algorithm
        /// Calculates (base^exponent) mod modulus in O(log exponent) time
        /// </summary>
        /// <param name="baseValue">The base value to exponentiate</param>
        /// <param name="exponent">The exponent (power to raise base to)</param>
        /// <param name="modulus">The modulus for the operation</param>
        /// <returns>(base^exponent) mod modulus</returns>
        private long ComputeModularExponentiation(long baseValue, int exponent, int modulus)
        {
            // Handle edge cases
            if (modulus == 1) return 0;
            if (exponent == 0) return 1;

            long result = 1;
            baseValue = baseValue % modulus; // Reduce base to avoid overflow

            // Square-and-multiply algorithm
            while (exponent > 0)
            {
                // If current bit is set, multiply result by current base
                if ((exponent & 1) == 1)
                {
                    result = (result * baseValue) % modulus;
                }

                // Square the base for next bit position
                baseValue = (baseValue * baseValue) % modulus;

                // Shift exponent right by 1 bit (equivalent to dividing by 2)
                exponent >>= 1;
            }

            return result;
        }

        /// <summary>
        /// Computes modular multiplicative inverse using the Extended Euclidean Algorithm
        /// Finds x such that (a * x) ≡ 1 (mod m), i.e., x = a^(-1) mod m
        /// </summary>
        /// <param name="value">The value to find inverse of</param>
        /// <param name="modulus">The modulus</param>
        /// <returns>The modular inverse of value mod modulus</returns>
        /// <exception cref="InvalidOperationException">Thrown when modular inverse doesn't exist</exception>
        private long ComputeModularInverse(long value, int modulus)
        {
            // Handle edge cases
            if (modulus <= 0)
                throw new ArgumentException("Modulus must be positive", nameof(modulus));
            if (modulus == 1)
                return 0;

            // Store original modulus for later use
            int originalModulus = modulus;
            long temp, quotient;

            // Extended Euclidean Algorithm variables
            // x0 and x1 track the coefficients in the linear combination
            long previousX = 0, currentX = 1;

            // Apply Extended Euclidean Algorithm
            while (value > 1)
            {
                quotient = value / modulus;

                // Update value and modulus for next iteration
                temp = modulus;
                modulus = (int)(value % modulus);
                value = temp;

                // Update coefficients
                temp = previousX;
                previousX = currentX - quotient * previousX;
                currentX = temp;
            }

            // Check if inverse exists (gcd must be 1)
            if (value != 1)
                throw new InvalidOperationException($"Modular inverse does not exist for {value} mod {originalModulus}");

            // Ensure positive result
            if (currentX < 0)
                currentX += originalModulus;

            return currentX;
        }
    }
}