using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q;
            //cipher = m ^ e mod n
            int cipher = fastModularExponintailMethod(M, e, n);
            return cipher;
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            int euler = (p - 1) * (q - 1);
            int d = findD(e, euler);
            int n = p * q;
            int plain = fastModularExponintailMethod(C, d, n);
            return plain;
        }

        public static int fastModularExponintailMethod(int baseNumber, int exp, int mod)
        {
            String binaryExp = Convert.ToString(exp, 2);
            long baseNumberTemp = baseNumber;

            for (int i = 0; i < binaryExp.Length - 1; i++)
            {
                if (binaryExp[i + 1] == '1')
                {
                    baseNumberTemp = (baseNumberTemp * baseNumberTemp) % mod;
                    baseNumberTemp = (baseNumberTemp * baseNumber) % mod;
                }
                else
                {
                    baseNumberTemp = (baseNumberTemp * baseNumberTemp) % mod;
                }
            }
            return (int)baseNumberTemp;
        }

        public static int findD(int e, int euler)
        {
            int counter = 0;
            while (true)
            {
                int equation = (e * counter) % euler;
                if (equation == 1)
                {
                    break;
                }
                else
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}
