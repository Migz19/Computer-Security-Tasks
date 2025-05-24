using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
    {
        
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            List<int> key = new List<int>() { i, j, k, l };
                            List<int> resultOfEncryption = Encrypt(plainText, key);

                            if (Enumerable.SequenceEqual(cipherText, resultOfEncryption))
                            {
                                return key;
                            }

                        }
                    }
                }
            }
            throw new InvalidAnlysisException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {

            int state = key.Count() % 2 == 0 ? 2 : 3;
            int[,] cipherMatrix = convertToMatrix(cipherText, state, cipherText.Count() / state, true);
            int[,] keyMatrix = convertToMatrix(key, state, state, false);

            List<int> plainText = new List<int>();

            //displayMatrix(cipherMatrix);

            if (state == 2)
            {
                int det = det2by2(keyMatrix);
                int mod = Mod(det, 26);
                if (GCD(26, Math.Abs(det)) != 1 || det == 0)
                {
                    throw new InvalidAnlysisException();
                }
                int b = calculateB(mod);
                Console.WriteLine(b);

                int[,] keyInverse = inverse2by2matrix(keyMatrix);
                //displayMatrix(keyMatrix);
                //displayMatrix(keyInverse);

                int[,] mat = multiblyMatrixbyNumber(keyInverse, b);
                //displayMatrix(mat);

                List<int> multiblication = new List<int>();
                //Console.WriteLine();
                for (int i = 0; i < cipherMatrix.GetLength(1); i++)
                {
                    int[,] co1OfPlain = extractColumns(cipherMatrix, cipherMatrix.GetLength(0), i);

                    multiblication = matrixMultiblication(mat, co1OfPlain);
                    for (int j = 0; j < multiblication.Count(); j++)
                    {
                        plainText.Add(multiblication[j]);
                        Console.Write(multiblication[j] + " ");
                    }
                }

            }
            else if (state == 3)
            {
                int det = det3by3(keyMatrix);

                if (GCD(26, Math.Abs(det)) != 1 || det == 0)
                {
                    throw new InvalidAnlysisException();
                }

                int mod = Mod(det, 26);
                int b = calculateB(mod);

                //mod = mod -26;

                int[,] keyInverse = inverse3by3matrix(keyMatrix, b);

                Console.WriteLine("det: " + det);
                Console.WriteLine("mod: " + mod);
                Console.WriteLine("b: " + b);

                //displayMatrix(keyInverse);
                Console.WriteLine();


                int[,] transposedMatrix = transpose(keyInverse);
                displayMatrix(transposedMatrix);


                List<int> multiblication = new List<int>();

                //displayMatrix(keyInverse);

                Console.WriteLine();

                for (int i = 0; i < cipherMatrix.GetLength(1); i++)
                {
                    int[,] co1OfPlain = extractColumns(cipherMatrix, cipherMatrix.GetLength(0), i);

                    multiblication = matrixMultiblication(transposedMatrix, co1OfPlain);
                    for (int j = 0; j < multiblication.Count(); j++)
                    {
                        plainText.Add(multiblication[j]);
                        //Console.WriteLine(multiblication[j] + " ");
                    }
                }


            }
            return plainText;
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> cipherText = new List<int>();
            int keyrows, keycols, plainrows;

            if (key.Count % 2 == 0)
            {
                keyrows = keycols = plainrows = 2;
            }
            else
            {
                keyrows = keycols = plainrows = 3;

            }
            int plaincols = plainText.Count / plainrows;



            int[,] plainMatrix = convertToMatrix(plainText, plainrows, plaincols, true);
            int[,] keyMatrix = convertToMatrix(key, keyrows, keycols, false);

            Console.WriteLine("key: ");
            displayMatrix(keyMatrix);
            Console.WriteLine("plain text: ");
            displayMatrix(plainMatrix);
            Console.WriteLine();

            List<int> cipherOut = new List<int>();
            int index = 0;


            List<int> multiblication = new List<int>();

            for (int i = 0; i < plainMatrix.GetLength(1); i++)
            {
                int[,] co1OfPlain = extractColumns(plainMatrix, plainMatrix.GetLength(0), i);

                multiblication = matrixMultiblication(keyMatrix, co1OfPlain);
                for (int j = 0; j < multiblication.Count(); j++)
                {
                    cipherOut.Add(multiblication[j]);
                }
    ;
            }

            //for (int j = 0; j < cipherOut.Count(); j++)
            //{
            //    Console.WriteLine(cipherOut[j]);
            //}

            return cipherOut;
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            //throw new NotImplementedException();

            int[,] plainMatrix = convertToMatrix(plainText, 3, plainText.Count() / 3, true);
            int[,] cipherMatrix = convertToMatrix(cipherText, 3, cipherText.Count() / 3, true);

            int det = det3by3(plainMatrix);
            int mod = Mod(det, 26);
            int b = calculateB(mod);

            if (GCD(26, Math.Abs(det)) != 1 || det == 0)
            {
                throw new InvalidAnlysisException();
            }

            int[,] plainMatInversed = inverse3by3matrix(plainMatrix, b);

            int[,] transposed = transpose(plainMatInversed);

            //displayMatrix(transposed);
            //Console.WriteLine();

            int[,] multiblication = new int[3, 3];

            List<int> result = new List<int>();
            //displayMatrix(cipherMatrix);
            //Console.WriteLine();
            //for (int i = 0; i < cipherMatrix.GetLength(1); i++)
            //{
            //int[,] co1OfPlain = extractColumns(transposed, transposed.GetLength(0), i);

            //displayMatrix(co1OfPlain);
            multiblication = matrixMultiplication(cipherMatrix, transposed);
            //Console.WriteLine();

            //for (int j = 0; j < multiblication.Count(); j++)
            //{
            //    //Console.WriteLine("res: ");
            //    result.Add(multiblication[j]);
            //    Console.WriteLine(multiblication[j] + " ");
            //}
            //}

            //displayMatrix(multiblication);

            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    result.Add(multiblication[j, i]);
                    //Console.WriteLine(multiblication[j, i]);
                }
            }

            return result;
        }


        #region
        // Helper functions
        // *******************************************************************************

        //flag: True for plain, false for key --> plain: fill cols firs. 
        public static int[,] convertToMatrix(List<int> list, int rows, int cols, bool flag)
        {
            int[,] matrix = new int[rows, cols];

            if (flag)
            {
                int index = 0;
                for (int j = 0; j < cols; j++)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        matrix[i, j] = list[index];
                        //Console.WriteLine(matrix[i, j]);
                        index++;
                    }
                }
            }
            else
            {
                int index = 0;
                for (int j = 0; j < cols; j++)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        matrix[j, i] = list[index];
                        //Console.WriteLine(matrix[i, j]);
                        index++;
                    }
                }
            }




            return matrix;
        }

        public static void displayMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }

        }

        // for matrix * matrix
        public static int[,] matrixMultiplication(int[,] m1, int[,] m2)
        {
            int[,] result = new int[3, 3];

            for (int i = 0; i < 3; i++)  // Rows of m1
            {
                for (int j = 0; j < 3; j++)  // Columns of m2
                {
                    int sum = 0;
                    for (int k = 0; k < 3; k++)  // Iterate through row-column pairs
                    {
                        sum += m1[i, k] * m2[k, j];
                    }
                    result[i, j] = sum % 26;  // Apply mod 26 to keep within alphabetic range
                }
            }

            return result;
        }


        // for matrix * col
        public static List<int> matrixMultiblication(int[,] m1, int[,] m2)
        {
            //int[,] output = new int[m1.GetLength(1), m2.GetLength(0)];

            List<int> output = new List<int>();

            for (int i = 0; i < m2.GetLength(0); i++)
            {
                int sum = 0;
                for (int j = 0; j < m1.GetLength(1); j++)
                {
                    //Console.Write(m1[i, j] + " " + m2[j, 0] + "\n");
                    sum += m1[i, j] * m2[j, 0];
                }
                sum %= 26;
                //Console.WriteLine(sum);
                output.Add(sum);

            }

            return output;
        }

        public static int[,] extractColumns(int[,] matrix, int rows, int index)
        {
            int[,] newMatrix = new int[rows, 1];


            for (int j = 0; j < 1; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    newMatrix[i, j] = matrix[i, index];
                }
            }

            //displayMatrix(newMatrix);

            return newMatrix;
        }

        public static int det2by2(int[,] matrix)
        {
            int det = (matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]);
            return det;
        }


        public static int det3by3(int[,] matrix)
        {
            int det = (matrix[0, 0] * ((matrix[1, 1] * matrix[2, 2]) - (matrix[1, 2] * matrix[2, 1])))
            - (matrix[0, 1] * ((matrix[1, 0] * matrix[2, 2]) - (matrix[1, 2] * matrix[2, 0])))
            + (matrix[0, 2] * ((matrix[1, 0] * matrix[2, 1]) - (matrix[1, 1] * matrix[2, 0])));

            return det;
        }

        public static int[,] inverse2by2matrix(int[,] matrix)
        {
            int[,] output = new int[2, 2];

            output[0, 0] = matrix[1, 1];
            output[1, 1] = matrix[0, 0];

            output[0, 1] = -1 * matrix[0, 1];
            output[1, 0] = -1 * matrix[1, 0];

            return output;
        }

        public static int calculateB(int b)
        {
            for (int i = 1; i < 26; i++)
                if (((b % 26) * (i % 26)) % 26 == 1)
                    return i;
            return 1;
        }

        public static int GCD(int a, int b)
        {
            if (b == 0) return a;
            return GCD(b, a % b);
        }

        public static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        public static int[,] inverse3by3matrix(int[,] matrix, int b)
        {
            int[,] outputMatrix = new int[3, 3];

            for (int z = 0; z < 3; z++)
            {
                for (int k = 0; k < 3; k++)
                {
                    int[,] coMatrix = new int[2, 2];
                    int p = 0, t = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (i != z && j != k)
                            {
                                coMatrix[p, t % 2] = matrix[i, j];
                                t++;
                                if (t % 2 == 0)
                                    p++;
                            }
                        }
                    }

                    outputMatrix[z, k] = Mod((int)Math.Pow(-1, z + k) * b * det2by2(coMatrix), 26);
                }
            }

            return outputMatrix;
        }

        public static int[,] transpose(int[,] matrix)
        {
            int size = matrix.GetLength(0); 
            int[,] output = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    output[j, i] = matrix[i, j]; 
                }
            }

            return output;

        }
        public static int[,] multiblyMatrixbyNumber(int[,] matrix, int num)
        {
            int[,] output = new int[2, 2];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    output[i, j] = Mod((matrix[i, j] * num), 26);
                }
            }

            return output;
        }

        #endregion
    }
}
