using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{/// <summary>
/// hallo world
/// huyhallowo
/// </summary>
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        // Encrypt using Auto-Key Vigenere Cipher
        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToUpper();
            key = key.ToUpper();
            StringBuilder cipherText = new StringBuilder();

            // Extend the key with the plaintext itself
            string extendedKey = key + plainText;

            for (int i = 0; i < plainText.Length; i++)
            {
                int plainIndex = Alphabet.IndexOf(plainText[i]);
                int keyIndex = Alphabet.IndexOf(extendedKey[i]);

                if (plainIndex == -1) continue; // Ignore non-alphabetic characters

                int cipherIndex = (plainIndex + keyIndex) % 26;
                cipherText.Append(Alphabet[cipherIndex]);
            }

            return cipherText.ToString();
        }

        // Decrypt using Auto-Key Vigenere Cipher
        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();
            StringBuilder plainText = new StringBuilder();
            StringBuilder fullKey = new StringBuilder(key);

            for (int i = 0; i < cipherText.Length; i++)
            {
                int cipherIndex = Alphabet.IndexOf(cipherText[i]);
                int keyIndex = Alphabet.IndexOf(fullKey[i]);

                if (cipherIndex == -1) continue; // Ignore non-alphabetic characters

                int plainIndex = (cipherIndex - keyIndex + 26) % 26;
                char decryptedChar = Alphabet[plainIndex];

                plainText.Append(decryptedChar);
                fullKey.Append(decryptedChar); // Add decrypted character to the key for next step
            }

            return plainText.ToString();
        }

        // Analyze method to retrieve the key given the plaintext and ciphertext
        public string Analyse(string plainText, string cipherText)
        {

            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();

            StringBuilder key = new StringBuilder();

            for (int i = 0; i < cipherText.Length; i++)
            {
                int cipherIndex = Alphabet.IndexOf(cipherText[i]);
                int plainIndex = Alphabet.IndexOf(plainText[i]);

                if (cipherIndex == -1 || plainIndex == -1) continue; // Ignore non-alphabetic characters

                int keyIndex = (cipherIndex - plainIndex + 26) % 26;
                char keyChar = Alphabet[keyIndex];

                key.Append(keyChar);


            }
            string temp = "";
            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] == plainText[0])
                {
                    break;
                }
                else
                {
                    temp += key[i];
                }
            }
            Console.WriteLine(temp.ToString().ToLower());
            return temp.ToString().ToLower(); // Return full derived key
        }
    }


}
