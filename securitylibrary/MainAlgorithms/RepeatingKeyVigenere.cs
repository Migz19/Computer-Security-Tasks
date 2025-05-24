using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        public string Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            int cipherLength = cipherText.Length;

            string key = GenerateKey(plainText, cipherText, alphabet, cipherLength);
            string initialKey = key.Substring(0, 1);

            return FindKey(plainText, cipherText, alphabet, key, initialKey);
        }

        private string GenerateKey(string plainText, string cipherText, string alphabet, int cipherLength)
        {
            string key = "";
            for (int i = 0; i < cipherLength; i++)
            {
                int newIndex = (alphabet.IndexOf(cipherText[i]) - alphabet.IndexOf(plainText[i]) + alphabet.Length) % alphabet.Length;
                key += alphabet[newIndex];
            }
            return key;
        }


        private string FindKey(string plainText, string cipherText, string alphabet, string key, string initialKey)
        {
            int keyLength = key.Length;
            for (int i = 1; i < keyLength; i++)
            {
                if (cipherText.Equals(Encrypt(plainText, initialKey)))
                {
                    return initialKey;
                }
                initialKey += key[i];
            }
            return key;
        }


        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            int cipherLength = cipherText.Length;
            string plaintext = "";
            string repeatedKey = RepeatKey(key, cipherLength);

            for (int i = 0; i < cipherLength; i++)
            {
                int newIndex = (alphabet.IndexOf(cipherText[i]) - alphabet.IndexOf(repeatedKey[i]) + alphabet.Length) % alphabet.Length;
                plaintext += alphabet[newIndex];
            }
            return plaintext;
        }


        private string RepeatKey(string key, int length)
        {
            string repeatedKey = key;
            while (repeatedKey.Length < length)
            {
                repeatedKey += key;
            }
            return repeatedKey;
        }

        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToLower();
            int plainLength = plainText.Length;
            string ciphertext = "";
            string repeatedKey = RepeatKey(key, plainLength);

            for (int i = 0; i < plainLength; i++)
            {
                int newIndex = (alphabet.IndexOf(plainText[i]) + alphabet.IndexOf(repeatedKey[i])) % alphabet.Length;
                ciphertext += alphabet[newIndex];
            }
            return ciphertext;
        }
    }
}