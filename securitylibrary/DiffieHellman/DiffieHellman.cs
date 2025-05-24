using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman
    {
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            /*
             * Q is an Modulus public value 
             * Alpha is the base and its an public value as well 
             * we have two users  a,b 
             * randomely genrete the secret keys  xa , xb (fourtunatly its passed already in testing )
             * helper function that calculate ya , yb
             * anathor helper function calculate final key passed to it an 
             */
            var result = new List<int>();
            int Ya = GetY(alpha, xa, q);
            int Yb = GetY(alpha, xb, q);
            int Ka = GetSecretKey(Yb, xa, q);
            int Kb = GetSecretKey(Ya, xb, q);
            result.Add(Ka);
            result.Add(Kb);
            return result;

        }

        // Efficient modular exponentiation
        private int ModPow(int baseVal, int exponent, int modulus)
        {
            long result = 1;
            long baseLong = baseVal % modulus;

            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                    result = (result * baseLong) % modulus;

                baseLong = (baseLong * baseLong) % modulus;
                exponent >>= 1;
            }

            return (int)result;
        }
        public int GetY(int alpha, int x, int q)
        {
            return ModPow(alpha, x, q);
        }

        public int GetSecretKey(int y, int x, int q)
        {
            return ModPow(y, x, q);
        }

    }
}
