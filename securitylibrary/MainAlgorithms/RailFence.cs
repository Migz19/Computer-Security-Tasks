using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {

            plainText = plainText.Replace(" ", "").ToUpper();
            cipherText = cipherText.Replace(" ", "").ToUpper();
            for (int key = 2; key <= plainText.Length; key++)
            {
                string encrypted = Encrypt(plainText, key).Replace("\0", ""); 
                
                if (cipherText.Equals(encrypted, StringComparison.Ordinal))
                {
                    return key;
                }
            }
            return 0;
        }


        public string Decrypt(string cipherText, int key)
        {
            cipherText = cipherText.Replace(" ", "");
            double ctLength = cipherText.Length;

            int col = (int)Math.Ceiling(ctLength / key);
            int index = 0;
            char[,] mat = new char[key, col];

            for (int r = 0; r < key; r++)
            {
                for (int c = 0; c < col; c++)
                {

                    mat[r, c] = cipherText[index];
                    index++;
                    if (index == ctLength)
                        break;
                }

            }
            string ptText = "";
            for (int c = 0; c < col; c++)
            {
                for (int r = 0; r < key; r++)
                {
                    ptText += mat[r, c];
                }
            }

            return ptText;
        }

        public string Encrypt(string plainText, int key)
        {

            plainText = plainText.Replace(" ", "");
            double ptLength = plainText.Length;

            int col = (int)Math.Ceiling(ptLength / key);
            int index = 0;
            char[,] mat = new char[key, col];

            for (int c = 0; c < col; c++)
            {

                for (int r = 0; r < key; r++)
                {

                    mat[r, c] = plainText[index];
                    index++;
                    if (index == ptLength)
                        break;
                }

            }
            string cipherText = "";
            for (int r = 0; r < key; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    cipherText += mat[r, c];
                }
            }

            return cipherText;
        }
   
    
    }
}
