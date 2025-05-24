using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        private static List<int[]> permutation = new List<int[]>();

        public List<int> Analyse(string plainText, string cipherText)
        {
            void GeneratePermutations()
            {
                void Permututation(int[] list, int k, int m, int sz)
                {
                    void Swap(ref int a, ref int b)
                    {
                        int Temp = a;
                        a = b;
                        b = Temp;
                    }

                    if (k == m)
                    {
                        int[] arr = new int[sz];
                        for (var i = 0; i <= m; i++)
                            arr[i] = list[i];
                        permutation.Add(arr);
                    }
                    else
                    {
                        for (var i = k; i <= m; i++)
                        {
                            Swap(ref list[k], ref list[i]);
                            Permututation(list, k + 1, m, sz);
                            Swap(ref list[k], ref list[i]);
                        }
                    }
                }

                for (var i = 2; i <= 7; i++)
                {
                    int[] arr = new int[i];
                    for (var j = 1; j <= i; j++)
                    {
                        arr[j - 1] = j;
                    }
                    Permututation(arr, 0, i - 1, i);
                }
            }

            GeneratePermutations();

            List<int> Key = new List<int>();
            for (var i = 0; i < permutation.Count; i++)
            {
                Key = permutation[i].ToList();
                if (plainText.ToLower() == Decrypt(cipherText.ToLower(), Key))
                {
                    break;
                }
            }

            return Key;
        }


        public string Decrypt(string cipherText, List<int> key)
        {
            Dictionary<int, List<char>> columns = new Dictionary<int, List<char>>();
            double cipherlength = Convert.ToDouble(cipherText.Length);
            double numofcols = Convert.ToDouble(key.Count);
            double numofrows = Math.Ceiling((cipherlength / numofcols));
            string output = "";
            int count = 0;
            for (var i = 0; i < numofcols; i++)
            {
                columns.Add(i + 1, new List<char>());
                for (var j = 0; j < numofrows; j++)
                {

                    if (count >= cipherText.Length)
                    {
                        columns[i + 1].Add(' ');
                        continue;
                    }
                    columns[i + 1].Add(cipherText[count]);
                    count++;

                }
            }
            for (var i = 0; i < numofrows; i++)
            {
                for (var j = 0; j < numofcols; j++)
                {
                    if (columns[key[j]][i] == ' ')
                    {
                        continue;
                    }
                    output = output + columns[key[j]][i];

                }
            }

            return output;
        }

        public string Encrypt(string plainText, List<int> key)
        {

            Dictionary<int, List<char>> columns = new Dictionary<int, List<char>>();
            string output = "";
            double plainlength = Convert.ToDouble(plainText.Length);
            double keylength = Convert.ToDouble(key.Count);
            double colSize = Math.Ceiling((plainlength / keylength));
            var count = 0;
            var j = 0;
            for (var i = 0; i < key.Count; i++)
            {
             
                columns.Add(key[i], new List<char>());
                for (var u = 0; u < colSize; u++)
                {
                    if (count == plainlength)
                    {
                        columns[key[i]].Add(' ');
                        continue;
                    }

                    columns[key[i]].Add(plainText[j]);
                    j += key.Count;
                    if (j >= plainText.Length)
                    {
                        j = i + 1;
                    }
                    count++;

                }
            }
            for (var i = 1; i <= key.Count; i++)
            {
                for (var k = 0; k < colSize; k++)
                {
                    if (columns[i][k] == ' ')
                    {
                        continue;
                    }
                   
                    output = output + columns[i][k];
                }
            }
            return output.ToUpper();
        }
    }
}
