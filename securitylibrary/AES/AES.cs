using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            string plainText = "0x";
            string cipherText_after_remove_0x = cipherText.Substring(2);
            string key_after_remove_0x = key.Substring(2);
            Console.WriteLine(key_after_remove_0x);
            string[,] initilaKeyMatrix = new string[4, 4];
            //filling the initial matrix of the key
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    initilaKeyMatrix[j, i] = key_after_remove_0x.Substring(0, 2);
                    //Console.WriteLine($"i have added the {key_after_remove_0x.Substring(0, 2)}"); 
                    key_after_remove_0x = key_after_remove_0x.Substring(2);
                    //Console.WriteLine($"updated key{key_after_remove_0x}");
                }
            }
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        Console.Write(initilaKeyMatrix[i, j] + " ");
            //    }
            //    Console.WriteLine();
            //}
            List<string[,]> generated_keys = new List<string[,]>();
            //this is the loop for generating the keys
            generated_keys.Add(initilaKeyMatrix);
            for (int i = 1; i <= 10; i++)
            {
                string[,] previous_key_matrix = generated_keys.Last();
                string[,] keyMatrix = new string[4, 4];
                for (int j = 0; j < 4; j++)
                {
                    //this handle the way to get the w0 for the key 
                    if (j == 0)
                    {
                        string word0_prvious_key = "";
                        string word3_of_prvious_key = "";
                        for (int k = 0; k < 4; k++)
                        {
                            word0_prvious_key += previous_key_matrix[k, 0];
                            word3_of_prvious_key += previous_key_matrix[k, 3];
                        }

                        word3_of_prvious_key = RotWord(word3_of_prvious_key);
                        string rcon = Rcon[i];
                        //now apply the subBytes for the word3
                        string word3_after_subBytes = "";
                        for (int k = 0; k < 4; k++)
                        {
                            int row = Convert.ToInt32(word3_of_prvious_key[0].ToString(), 16);
                            int col = Convert.ToInt32(word3_of_prvious_key[1].ToString(), 16);
                            word3_after_subBytes += SBox2D[row, col];
                            word3_of_prvious_key = word3_of_prvious_key.Substring(2);
                        }
                        //Console.WriteLine(word0_prvious_key);
                        //Console.WriteLine(word3_after_subBytes);
                        //Console.WriteLine(rcon);
                        string word0_prvious_key_binary = HexToBinary(word0_prvious_key);
                        string word3_after_subBytes_binary = HexToBinary(word3_after_subBytes);
                        string rcon_binary = HexToBinary(rcon);
                        string xor_of_word0_with_word3 = "";
                        for (int k = 0; k < word3_after_subBytes_binary.Length; k++)
                        {
                            if (word0_prvious_key_binary[k] == word3_after_subBytes_binary[k])
                            {
                                xor_of_word0_with_word3 += "0";
                            }
                            else
                            {
                                xor_of_word0_with_word3 += "1";
                            }
                        }
                        string xor_of_word0_and_word_3_with_rcon = "";
                        //Console.WriteLine($"this is the rcon binary for round {i} : {rcon_binary}");
                        //Console.WriteLine($"this is the value where the rcon will be xor with: {xor_of_word0_with_word3} ");
                        for (int k = 0; k < xor_of_word0_with_word3.Length; k++)
                        {
                            if (k < 8)
                            {
                                if (xor_of_word0_with_word3[k] == rcon_binary[k])
                                {
                                    xor_of_word0_and_word_3_with_rcon += "0";
                                }
                                else
                                {
                                    xor_of_word0_and_word_3_with_rcon += "1";
                                }
                            }
                            else
                            {
                                xor_of_word0_and_word_3_with_rcon += xor_of_word0_with_word3[k];
                            }

                        }
                        string word0_for_current_key = BinaryToHex(xor_of_word0_and_word_3_with_rcon);
                        for (int k = 0; k < 4; k++)
                        {
                            keyMatrix[k, j] = word0_for_current_key.Substring(0, 2);
                            word0_for_current_key = word0_for_current_key.Substring(2);
                        }
                        //Console.WriteLine($"this is the word 0 for the first key {word0_for_current_key}");
                    }
                    //here we will get the w1 and w2 and w3 for our key
                    else
                    {
                        string word_of_previous_key = "";
                        string word_of_current_key = "";
                        for (int k = 0; k < 4; k++)
                        {
                            word_of_previous_key += previous_key_matrix[k, j];
                        }
                        for (int k = 0; k < 4; k++)
                        {
                            word_of_current_key += keyMatrix[k, j - 1];
                        }
                        string word_of_previous_key_binary = HexToBinary(word_of_previous_key);
                        string word_of_current_key_binary = HexToBinary(word_of_current_key);

                        string word_of_previous_key_binary_XOR_word_of_current_key_binary = "";
                        for (int k = 0; k < word_of_current_key_binary.Length; k++)
                        {
                            if (word_of_current_key_binary[k] == word_of_previous_key_binary[k])
                            {
                                word_of_previous_key_binary_XOR_word_of_current_key_binary += "0";
                            }
                            else
                            {
                                word_of_previous_key_binary_XOR_word_of_current_key_binary += "1";
                            }
                        }
                        string word = BinaryToHex(word_of_previous_key_binary_XOR_word_of_current_key_binary);
                        for (int k = 0; k < 4; k++)
                        {
                            keyMatrix[k, j] = word.Substring(0, 2);
                            word = word.Substring(2);
                        }
                    }
                }
                generated_keys.Add(keyMatrix);

            }
            //print the keys that have been generated
            //foreach (string[,] Matrix in generated_keys)
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        for (int j = 0; j < 4; j++)
            //        {
            //            Console.Write(Matrix[i, j] + " ");
            //        }
            //        Console.WriteLine();
            //    }
            //    Console.WriteLine("This is the next key");
            //}
            //now we let's start with the encryption process
            string[,] stateMatrix = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    stateMatrix[j, i] = cipherText_after_remove_0x.Substring(0, 2);
                    cipherText_after_remove_0x = cipherText_after_remove_0x.Substring(2);
                }
            }
            //Console.WriteLine("this is the state matrix");
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        Console.Write(stateMatrix[i, j] + " ");
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine("This is after adding the round key");
            AddRoundKey(stateMatrix, generated_keys[10]);
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        Console.Write(stateMatrix[i, j] + " ");
            //    }
            //    Console.WriteLine();
            //}
            string[,] finalMatrix = new string[4, 4];
            for (int i = 9; i >= 0; i--)
            {
                string[,] keyMatrix = generated_keys[i];
                string[,] matrix_after_subBytes = new string[4, 4];
                string[,] matrix_after_shift_rows = new string[4, 4];
                string[,] matrix_after_mix_columns = new string[4, 4];
                string[,] matrix_after_Add_round_key = new string[4, 4];

                InvShiftRows(stateMatrix);
                matrix_after_shift_rows = stateMatrix;
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        int row = Convert.ToInt32(matrix_after_shift_rows[j, k][0].ToString(), 16);
                        int col = Convert.ToInt32(matrix_after_shift_rows[j, k][1].ToString(), 16);
                        matrix_after_subBytes[j, k] = InvSBox2D[row, col];
                    }
                }

                if (i > 0)
                {
                    AddRoundKey(matrix_after_subBytes, keyMatrix);
                    matrix_after_Add_round_key = matrix_after_subBytes;
                    stateMatrix = InvMixColumns(matrix_after_Add_round_key);
                }
                else
                {
                    AddRoundKey(matrix_after_subBytes, keyMatrix);
                    finalMatrix = matrix_after_subBytes;
                }

            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    plainText += finalMatrix[j, i];
                }
                Console.WriteLine();
            }
            return plainText;
        }

        public override string Encrypt(string plainText, string key)
        {
            string cipher = "0x";
            string plainText_after_remove_0x = plainText.Substring(2);
            string key_after_remove_0x = key.Substring(2);
            Console.WriteLine(key_after_remove_0x);
            string[,] initilaKeyMatrix = new string[4, 4];
            //filling the initial matrix of the key
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    initilaKeyMatrix[j, i] = key_after_remove_0x.Substring(0, 2);
                    //Console.WriteLine($"i have added the {key_after_remove_0x.Substring(0, 2)}"); 
                    key_after_remove_0x = key_after_remove_0x.Substring(2);
                    //Console.WriteLine($"updated key{key_after_remove_0x}");
                }
            }
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        Console.Write(initilaKeyMatrix[i, j] + " ");
            //    }
            //    Console.WriteLine();
            //}
            List<string[,]> generated_keys = new List<string[,]>();
            //this is the loop for generating the keys
            generated_keys.Add(initilaKeyMatrix);
            for (int i = 1; i <= 10; i++)
            {
                string[,] previous_key_matrix = generated_keys.Last();
                string[,] keyMatrix = new string[4, 4];
                for (int j = 0; j < 4; j++)
                {
                    //this handle the way to get the w0 for the key 
                    if (j == 0)
                    {
                        string word0_prvious_key = "";
                        string word3_of_prvious_key = "";
                        for (int k = 0; k < 4; k++)
                        {
                            word0_prvious_key += previous_key_matrix[k, 0];
                            word3_of_prvious_key += previous_key_matrix[k, 3];
                        }

                        word3_of_prvious_key = RotWord(word3_of_prvious_key);
                        string rcon = Rcon[i];
                        //now apply the subBytes for the word3
                        string word3_after_subBytes = "";
                        for (int k = 0; k < 4; k++)
                        {
                            int row = Convert.ToInt32(word3_of_prvious_key[0].ToString(), 16);
                            int col = Convert.ToInt32(word3_of_prvious_key[1].ToString(), 16);
                            word3_after_subBytes += SBox2D[row, col];
                            word3_of_prvious_key = word3_of_prvious_key.Substring(2);
                        }
                        //Console.WriteLine(word0_prvious_key);
                        //Console.WriteLine(word3_after_subBytes);
                        //Console.WriteLine(rcon);
                        string word0_prvious_key_binary = HexToBinary(word0_prvious_key);
                        string word3_after_subBytes_binary = HexToBinary(word3_after_subBytes);
                        string rcon_binary = HexToBinary(rcon);
                        string xor_of_word0_with_word3 = "";
                        for (int k = 0; k < word3_after_subBytes_binary.Length; k++)
                        {
                            if (word0_prvious_key_binary[k] == word3_after_subBytes_binary[k])
                            {
                                xor_of_word0_with_word3 += "0";
                            }
                            else
                            {
                                xor_of_word0_with_word3 += "1";
                            }
                        }
                        string xor_of_word0_and_word_3_with_rcon = "";
                        //Console.WriteLine($"this is the rcon binary for round {i} : {rcon_binary}");
                        //Console.WriteLine($"this is the value where the rcon will be xor with: {xor_of_word0_with_word3} ");
                        for (int k = 0; k < xor_of_word0_with_word3.Length; k++)
                        {
                            if (k < 8)
                            {
                                if (xor_of_word0_with_word3[k] == rcon_binary[k])
                                {
                                    xor_of_word0_and_word_3_with_rcon += "0";
                                }
                                else
                                {
                                    xor_of_word0_and_word_3_with_rcon += "1";
                                }
                            }
                            else
                            {
                                xor_of_word0_and_word_3_with_rcon += xor_of_word0_with_word3[k];
                            }

                        }
                        string word0_for_current_key = BinaryToHex(xor_of_word0_and_word_3_with_rcon);
                        for (int k = 0; k < 4; k++)
                        {
                            keyMatrix[k, j] = word0_for_current_key.Substring(0, 2);
                            word0_for_current_key = word0_for_current_key.Substring(2);
                        }
                        //Console.WriteLine($"this is the word 0 for the first key {word0_for_current_key}");
                    }
                    //here we will get the w1 and w2 and w3 for our key
                    else
                    {
                        string word_of_previous_key = "";
                        string word_of_current_key = "";
                        for (int k = 0; k < 4; k++)
                        {
                            word_of_previous_key += previous_key_matrix[k, j];
                        }
                        for (int k = 0; k < 4; k++)
                        {
                            word_of_current_key += keyMatrix[k, j - 1];
                        }
                        string word_of_previous_key_binary = HexToBinary(word_of_previous_key);
                        string word_of_current_key_binary = HexToBinary(word_of_current_key);

                        string word_of_previous_key_binary_XOR_word_of_current_key_binary = "";
                        for (int k = 0; k < word_of_current_key_binary.Length; k++)
                        {
                            if (word_of_current_key_binary[k] == word_of_previous_key_binary[k])
                            {
                                word_of_previous_key_binary_XOR_word_of_current_key_binary += "0";
                            }
                            else
                            {
                                word_of_previous_key_binary_XOR_word_of_current_key_binary += "1";
                            }
                        }
                        string word = BinaryToHex(word_of_previous_key_binary_XOR_word_of_current_key_binary);
                        for (int k = 0; k < 4; k++)
                        {
                            keyMatrix[k, j] = word.Substring(0, 2);
                            word = word.Substring(2);
                        }
                    }
                }
                generated_keys.Add(keyMatrix);

            }
            //print the keys that have been generated
            //foreach (string[,] Matrix in generated_keys)
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        for (int j = 0; j < 4; j++)
            //        {
            //            Console.Write(Matrix[i, j] + " ");
            //        }
            //        Console.WriteLine();
            //    }
            //    Console.WriteLine("This is the next key");
            //}
            //now we let's start with the encryption process
            string[,] stateMatrix = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    stateMatrix[j, i] = plainText_after_remove_0x.Substring(0, 2);
                    plainText_after_remove_0x = plainText_after_remove_0x.Substring(2);
                }
            }
            //Console.WriteLine("this is the state matrix");
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        Console.Write(stateMatrix[i, j] + " ");
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine("This is after adding the round key");
            AddRoundKey(stateMatrix, generated_keys[0]);
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        Console.Write(stateMatrix[i, j] + " ");
            //    }
            //    Console.WriteLine();
            //}
            string[,] finalMatrix = new string[4, 4];
            for (int i = 1; i < 11; i++)
            {
                string[,] keyMatrix = generated_keys[i];
                string[,] matrix_after_subBytes = new string[4, 4];
                string[,] matrix_after_shift_rows = new string[4, 4];
                string[,] matrix_after_mix_columns = new string[4, 4];
                string[,] matrix_after_Add_round_key = new string[4, 4];
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        int row = Convert.ToInt32(stateMatrix[j, k][0].ToString(), 16);
                        int col = Convert.ToInt32(stateMatrix[j, k][1].ToString(), 16);
                        matrix_after_subBytes[j, k] = SBox2D[row, col];
                    }
                }
                ShiftRows(matrix_after_subBytes);
                matrix_after_shift_rows = matrix_after_subBytes;
                if (i < 10)
                {
                    matrix_after_mix_columns = MixColumns(matrix_after_shift_rows);
                    AddRoundKey(matrix_after_mix_columns, keyMatrix);
                    stateMatrix = matrix_after_mix_columns;
                }
                else
                {
                    AddRoundKey(matrix_after_shift_rows, keyMatrix);
                    finalMatrix = matrix_after_shift_rows;
                }

            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    cipher += finalMatrix[j, i];
                }
                Console.WriteLine();
            }
            return cipher;
        }
        private static readonly string[,] SBox2D = new string[16, 16] {
    { "63", "7C", "77", "7B", "F2", "6B", "6F", "C5", "30", "01", "67", "2B", "FE", "D7", "AB", "76" },
    { "CA", "82", "C9", "7D", "FA", "59", "47", "F0", "AD", "D4", "A2", "AF", "9C", "A4", "72", "C0" },
    { "B7", "FD", "93", "26", "36", "3F", "F7", "CC", "34", "A5", "E5", "F1", "71", "D8", "31", "15" },
    { "04", "C7", "23", "C3", "18", "96", "05", "9A", "07", "12", "80", "E2", "EB", "27", "B2", "75" },
    { "09", "83", "2C", "1A", "1B", "6E", "5A", "A0", "52", "3B", "D6", "B3", "29", "E3", "2F", "84" },
    { "53", "D1", "00", "ED", "20", "FC", "B1", "5B", "6A", "CB", "BE", "39", "4A", "4C", "58", "CF" },
    { "D0", "EF", "AA", "FB", "43", "4D", "33", "85", "45", "F9", "02", "7F", "50", "3C", "9F", "A8" },
    { "51", "A3", "40", "8F", "92", "9D", "38", "F5", "BC", "B6", "DA", "21", "10", "FF", "F3", "D2" },
    { "CD", "0C", "13", "EC", "5F", "97", "44", "17", "C4", "A7", "7E", "3D", "64", "5D", "19", "73" },
    { "60", "81", "4F", "DC", "22", "2A", "90", "88", "46", "EE", "B8", "14", "DE", "5E", "0B", "DB" },
    { "E0", "32", "3A", "0A", "49", "06", "24", "5C", "C2", "D3", "AC", "62", "91", "95", "E4", "79" },
    { "E7", "C8", "37", "6D", "8D", "D5", "4E", "A9", "6C", "56", "F4", "EA", "65", "7A", "AE", "08" },
    { "BA", "78", "25", "2E", "1C", "A6", "B4", "C6", "E8", "DD", "74", "1F", "4B", "BD", "8B", "8A" },
    { "70", "3E", "B5", "66", "48", "03", "F6", "0E", "61", "35", "57", "B9", "86", "C1", "1D", "9E" },
    { "E1", "F8", "98", "11", "69", "D9", "8E", "94", "9B", "1E", "87", "E9", "CE", "55", "28", "DF" },
    { "8C", "A1", "89", "0D", "BF", "E6", "42", "68", "41", "99", "2D", "0F", "B0", "54", "BB", "16" }
};
        public static string HexToBinary(string hexString)
        {
            // Remove any spaces or common hex prefixes if present
            hexString = hexString.Replace(" ", "").Replace("0x", "");

            StringBuilder binaryString = new StringBuilder();

            foreach (char hexChar in hexString)
            {
                string binaryDigit;

                switch (char.ToUpper(hexChar))
                {
                    case '0': binaryDigit = "0000"; break;
                    case '1': binaryDigit = "0001"; break;
                    case '2': binaryDigit = "0010"; break;
                    case '3': binaryDigit = "0011"; break;
                    case '4': binaryDigit = "0100"; break;
                    case '5': binaryDigit = "0101"; break;
                    case '6': binaryDigit = "0110"; break;
                    case '7': binaryDigit = "0111"; break;
                    case '8': binaryDigit = "1000"; break;
                    case '9': binaryDigit = "1001"; break;
                    case 'A': binaryDigit = "1010"; break;
                    case 'B': binaryDigit = "1011"; break;
                    case 'C': binaryDigit = "1100"; break;
                    case 'D': binaryDigit = "1101"; break;
                    case 'E': binaryDigit = "1110"; break;
                    case 'F': binaryDigit = "1111"; break;
                    default:
                        throw new ArgumentException($"Invalid hexadecimal character: {hexChar}");
                }

                binaryString.Append(binaryDigit);
            }

            return binaryString.ToString();
        }

        public static string BinaryToHex(string binaryString)
        {
            // Pad the binary string to make its length a multiple of 4 (for nibble alignment)
            binaryString = binaryString.PadLeft((binaryString.Length + 3) / 4 * 4, '0');

            // Convert binary to hexadecimal
            string hexString = "";
            for (int i = 0; i < binaryString.Length; i += 4)
            {
                string nibble = binaryString.Substring(i, 4);  // 4-bit chunk
                int decimalValue = Convert.ToInt32(nibble, 2);  // Binary to decimal
                hexString += decimalValue.ToString("X1");      // Decimal to hex (uppercase)
            }

            return hexString;
        }

        private static readonly string[] Rcon = new string[]
{
    "00000000",  // Unused
    "01000000",  // Round 1
    "02000000",  // Round 2
    "04000000",  // Round 3
    "08000000",  // Round 4
    "10000000",  // Round 5
    "20000000",  // Round 6
    "40000000",  // Round 7
    "80000000",  // Round 8
    "1B000000",  // Round 9
    "36000000"   // Round 10
};
        private static string RotWord(string word)
        {
            if (word.Length != 8)
                throw new ArgumentException("Word must be 8 hex characters (4 bytes)");

            return word.Substring(2) + word.Substring(0, 2);
        }
        public static void AddRoundKey(string[,] state, string[,] roundKey)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    byte stateByte = Convert.ToByte(state[row, col], 16);
                    byte keyByte = Convert.ToByte(roundKey[row, col], 16);
                    byte xorResult = (byte)(stateByte ^ keyByte);
                    state[row, col] = xorResult.ToString("X2"); // Format as hex with 2 digits
                }
            }
        }
        public static void ShiftRows(string[,] state)
        {
            for (int row = 1; row < 4; row++)
            {
                string[] tempRow = new string[4];
                for (int col = 0; col < 4; col++)
                {
                    tempRow[col] = state[row, (col + row) % 4];
                }
                for (int col = 0; col < 4; col++)
                {
                    state[row, col] = tempRow[col];
                }
            }
        }

        public static byte GFMul(byte a, byte b)
        {
            byte p = 0;
            for (int i = 0; i < 8; i++)
            {
                if ((b & 1) != 0)
                    p ^= a;
                bool hiBitSet = (a & 0x80) != 0;
                a <<= 1;
                if (hiBitSet)
                    a ^= 0x1B; // x^8 + x^4 + x^3 + x + 1
                b >>= 1;
            }
            return p;
        }

        public static string[,] MixColumns(string[,] state)
        {
            string[,] result = new string[4, 4];

            for (int c = 0; c < 4; c++)
            {
                byte[] col = new byte[4];
                for (int r = 0; r < 4; r++)
                {
                    col[r] = Convert.ToByte(state[r, c], 16);
                }

                byte r0 = (byte)(GFMul(0x02, col[0]) ^ GFMul(0x03, col[1]) ^ col[2] ^ col[3]);
                byte r1 = (byte)(col[0] ^ GFMul(0x02, col[1]) ^ GFMul(0x03, col[2]) ^ col[3]);
                byte r2 = (byte)(col[0] ^ col[1] ^ GFMul(0x02, col[2]) ^ GFMul(0x03, col[3]));
                byte r3 = (byte)(GFMul(0x03, col[0]) ^ col[1] ^ col[2] ^ GFMul(0x02, col[3]));

                result[0, c] = r0.ToString("X2");
                result[1, c] = r1.ToString("X2");
                result[2, c] = r2.ToString("X2");
                result[3, c] = r3.ToString("X2");
            }

            return result;
        }

        //Decryption helper functions
        private static readonly string[,] InvSBox2D = new string[16, 16]
{
    { "52", "09", "6A", "D5", "30", "36", "A5", "38", "BF", "40", "A3", "9E", "81", "F3", "D7", "FB" },
    { "7C", "E3", "39", "82", "9B", "2F", "FF", "87", "34", "8E", "43", "44", "C4", "DE", "E9", "CB" },
    { "54", "7B", "94", "32", "A6", "C2", "23", "3D", "EE", "4C", "95", "0B", "42", "FA", "C3", "4E" },
    { "08", "2E", "A1", "66", "28", "D9", "24", "B2", "76", "5B", "A2", "49", "6D", "8B", "D1", "25" },
    { "72", "F8", "F6", "64", "86", "68", "98", "16", "D4", "A4", "5C", "CC", "5D", "65", "B6", "92" },
    { "6C", "70", "48", "50", "FD", "ED", "B9", "DA", "5E", "15", "46", "57", "A7", "8D", "9D", "84" },
    { "90", "D8", "AB", "00", "8C", "BC", "D3", "0A", "F7", "E4", "58", "05", "B8", "B3", "45", "06" },
    { "D0", "2C", "1E", "8F", "CA", "3F", "0F", "02", "C1", "AF", "BD", "03", "01", "13", "8A", "6B" },
    { "3A", "91", "11", "41", "4F", "67", "DC", "EA", "97", "F2", "CF", "CE", "F0", "B4", "E6", "73" },
    { "96", "AC", "74", "22", "E7", "AD", "35", "85", "E2", "F9", "37", "E8", "1C", "75", "DF", "6E" },
    { "47", "F1", "1A", "71", "1D", "29", "C5", "89", "6F", "B7", "62", "0E", "AA", "18", "BE", "1B" },
    { "FC", "56", "3E", "4B", "C6", "D2", "79", "20", "9A", "DB", "C0", "FE", "78", "CD", "5A", "F4" },
    { "1F", "DD", "A8", "33", "88", "07", "C7", "31", "B1", "12", "10", "59", "27", "80", "EC", "5F" },
    { "60", "51", "7F", "A9", "19", "B5", "4A", "0D", "2D", "E5", "7A", "9F", "93", "C9", "9C", "EF" },
    { "A0", "E0", "3B", "4D", "AE", "2A", "F5", "B0", "C8", "EB", "BB", "3C", "83", "53", "99", "61" },
    { "17", "2B", "04", "7E", "BA", "77", "D6", "26", "E1", "69", "14", "63", "55", "21", "0C", "7D" }
};

        public static void InvShiftRows(string[,] state)
        {
            for (int row = 1; row < 4; row++)
            {
                string[] tempRow = new string[4];
                for (int col = 0; col < 4; col++)
                {
                    tempRow[(col + row) % 4] = state[row, col];
                }
                for (int col = 0; col < 4; col++)
                {
                    state[row, col] = tempRow[col];
                }
            }
        }

        public static string[,] InvMixColumns(string[,] state)
        {
            string[,] result = new string[4, 4];

            for (int c = 0; c < 4; c++)
            {
                byte[] col = new byte[4];
                for (int r = 0; r < 4; r++)
                {
                    col[r] = Convert.ToByte(state[r, c], 16);
                }

                byte r0 = (byte)(GFMul(0x0E, col[0]) ^ GFMul(0x0B, col[1]) ^ GFMul(0x0D, col[2]) ^ GFMul(0x09, col[3]));
                byte r1 = (byte)(GFMul(0x09, col[0]) ^ GFMul(0x0E, col[1]) ^ GFMul(0x0B, col[2]) ^ GFMul(0x0D, col[3]));
                byte r2 = (byte)(GFMul(0x0D, col[0]) ^ GFMul(0x09, col[1]) ^ GFMul(0x0E, col[2]) ^ GFMul(0x0B, col[3]));
                byte r3 = (byte)(GFMul(0x0B, col[0]) ^ GFMul(0x0D, col[1]) ^ GFMul(0x09, col[2]) ^ GFMul(0x0E, col[3]));

                result[0, c] = r0.ToString("X2");
                result[1, c] = r1.ToString("X2");
                result[2, c] = r2.ToString("X2");
                result[3, c] = r3.ToString("X2");
            }

            return result;
        }

    }
}
