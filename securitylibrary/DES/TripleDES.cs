using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal, not a string
    /// </summary>
    public class TripleDES : ICryptographicTechnique<string, List<string>>
    {
        private readonly DES des;

        public TripleDES()
        {
            des = new DES();
        }

        public string Encrypt(string plainText, List<string> keys)
        {
            string key1 = keys[0];
            string key2 = keys.Count > 1 ? keys[1] : key1;
            string key3 = keys.Count > 2 ? keys[2] : key1;

            string intermediateText1 = des.Encrypt(plainText, key1);
            string intermediateText2 = des.Decrypt(intermediateText1, key2);
            string cipherText = des.Encrypt(intermediateText2, key3);

            return cipherText;
        }

        public string Decrypt(string cipherText, List<string> keys)
        {
            string key1 = keys[0];
            string key2 = keys.Count > 1 ? keys[1] : key1;
            string key3 = keys.Count > 2 ? keys[2] : key1;

            string intermediateText1 = des.Decrypt(cipherText, key3);
            string intermediateText2 = des.Encrypt(intermediateText1, key2);
            string plainText = des.Decrypt(intermediateText2, key1);

            return plainText;
        }

        public List<string> Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }
    }
}