using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int l = alphabet.Length;
            var map = new Dictionary<char, char>();
            for (int i = 0; i < l; i++) 
            {
                var original_letter = alphabet[i];
                var Transformated_letter = alphabet[(i + key) % l];
                map.Add(original_letter, Transformated_letter); //now i have fully loaded map with each letter and corresponding transformed letter
            }

            //i will perform in the plain text which the text we need to encrypt
            var plain = plainText.ToUpper().ToCharArray();
            for (int i = 0; i < plain.Length; i++) 
            {
                if (map.ContainsKey(plain[i])) 
                {
                    plain[i] = map[plain[i]];
                }
            
            }
            return new string(plain);


            
        }

        public string Decrypt(string cipherText, int key)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int l = alphabet.Length;
            var map = new Dictionary<char, char>();
            for (int i = 0; i < l; i++)
            {
                var original_letter = alphabet[i];
                var Transformated_letter = alphabet[(i - key + l) % l];
                map.Add(original_letter, Transformated_letter); //now i have fully loaded map with each letter and corresponding transformed letter
            }
            var cipher = cipherText.ToUpper().ToCharArray();
            for (int i = 0; i < cipher.Length; i++) 
            {
                if (map.ContainsKey(cipher[i])) 
                {
                    cipher[i] = map[cipher[i]];
                }

            }
            return new string(cipher);
        }

        public int Analyse(string plainText, string cipherText)
        {
            plainText.ToUpper();
            cipherText.ToUpper();
            for (int key = 0; key < 26; key++) 
            {
                if (Encrypt(plainText, key) == cipherText) 
                {
                    return key;
                }

            }
            return -1;
        }
    }
}
