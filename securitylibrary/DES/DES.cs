using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class DES : CryptographicTechnique
    {

        public override string Decrypt(string cipherText, string key)
        {
            string PlainText = "0x";
            // first i will generate the 16 key and store them in an array
            string binary_key = HexToBinary(key);
            string c = "";
            string d = "";
            int[] shift = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            List<string> keys = new List<string>();
            string key_after_pc_1 = "";
            Console.WriteLine($"this is the binary key : {binary_key}");
            for (int i = 0; i < permutedChoice_1.Length; i++)
            {
                key_after_pc_1 += binary_key[permutedChoice_1[i] - 1].ToString();
            }
            //constructing the c and the d
            for (int i = 0; i < key_after_pc_1.Length; i++)
            {
                if (i < 28)
                {
                    c += key_after_pc_1[i];
                }
                else
                {
                    d += key_after_pc_1[i];
                }
            }
            //Console.WriteLine($"This is the key after pc1 {key_after_pc_1}");
            Console.WriteLine($"This is the c : {c} and this is the d : {d}");
            //in this loop we will apply the shift to  each of c and d and then apply pc2 and add the key to the array of keys
            for (int i = 0; i < 16; i++)
            {
                //Console.WriteLine($"this is after expantion permutaion {R_After_expantion_permutaion}");
                int shifts = shift[i];
                for (int j = 0; j < shifts; j++)
                {
                    c = c.Substring(1) + c[0];
                    d = d.Substring(1) + d[0];
                }
                string key_after_shift = c + d;
                //Console.WriteLine($"this is the shifted key\n{c}\n{d}");
                //now lets apply the permutation
                string key_after_pc_2 = "";
                for (int j = 0; j < permutedChoice_2.Length; j++)
                {
                    key_after_pc_2 += key_after_shift[permutedChoice_2[j] - 1];
                }
                keys.Add(key_after_pc_2);
            }

            //until this part we created the 16 key and stored them is the keys list

            //now we will start to deal with the cipher text to obtain the plain text
            //change the cipher to binary
            /*
             * 10000101 11101000 00010011 01010100 00001111 00001010 10110100 00000101
             * 11000110 10100100 11001011 10110000 00001101 00011000 00010001 01011000
             */
            string binary_cipher = HexToBinary(cipherText);
            Console.WriteLine($"cipher to binary : {binary_cipher}");
            //first lets apply the initial permutaion to the cipher

            string cipher_after_initial_permutation = "";
            for (int i = 0; i < initialPermutaion.Length; i++)
            {
                cipher_after_initial_permutation += binary_cipher[initialPermutaion[i] - 1];
            }
            Console.WriteLine($"this is after the apply of inverse final permutaion : {cipher_after_initial_permutation}");
            // lets split the cipher into two halfs l and r and swap them
            string L = cipher_after_initial_permutation.Substring(32);
            string R = cipher_after_initial_permutation.Substring(0, 32);
            Console.WriteLine($"this is the R : {R} and this is the L : {L} after we have done the swap");
            //now lets start to make the loop of decryption
            for (int i = 15; i >= 0; i--)
            {
                string temp_R = R;
                string temp_L = L;
                string expantion_permutaion_for_L = "";
                for (int j = 0; j < expantionPermutation.Length; j++)
                {
                    expantion_permutaion_for_L += L[expantionPermutation[j] - 1];
                }
                string xor_of_expantion_permutation_for_L_and_key = "";
                string round_key = keys[i];
                for (int j = 0; j < round_key.Length; j++)
                {
                    if (round_key[j] == expantion_permutaion_for_L[j])
                    {
                        xor_of_expantion_permutation_for_L_and_key += "0";
                    }
                    else
                    {
                        xor_of_expantion_permutation_for_L_and_key += "1";
                    }
                }
                //now lets go with the s-boxes
                string temp = xor_of_expantion_permutation_for_L_and_key;
                string out_of_the_s_boxes = "";
                for (int j = 0; j < 8; j++)
                {
                    string sub = temp.Substring(0, 6);

                    temp = temp.Substring(6);
                    //Console.WriteLine($"This is the substring {sub}");
                    int row_number = Convert.ToInt32(sub[0] + sub[sub.Length - 1].ToString(), 2);
                    int column_number = Convert.ToInt32(sub.Substring(1, 4), 2);
                    //Console.WriteLine($"this is the binary for the column {sub.Substring(1, 4)} and this is the binary for the row number {sub[0] + sub[sub.Length - 1].ToString()}");
                    int decimal_representation = SBoxes[j, row_number, column_number];
                    //Console.WriteLine($"This is the decimal number {decimal_representation}");
                    out_of_the_s_boxes += Convert.ToString(decimal_representation, 2).PadLeft(4, '0');
                }
                //apply permutation for the out of the s-boxes
                string permutaion_of_s_boxes = "";
                for (int j = 0; j < permutationFunction.Length; j++)
                {
                    permutaion_of_s_boxes += out_of_the_s_boxes[permutationFunction[j] - 1];
                }
                //xor of the permutation of the s boxes and the R to get the L
                string xor_of_permutation_of_s_boxes_with_R = "";
                for (int j = 0; j < R.Length; j++)
                {
                    if (R[j] == permutaion_of_s_boxes[j])
                    {
                        xor_of_permutation_of_s_boxes_with_R += "0";
                    }
                    else
                    {
                        xor_of_permutation_of_s_boxes_with_R += "1";
                    }
                }
                R = L;
                L = xor_of_permutation_of_s_boxes_with_R;

            }
            /*
             * 11001100 00000000 11001100 11111111 11110000 10101010 11110000 10101010
             * 11001100 00000000 11001100 11111111 11110000 10101010 11110000 10101010
             */
            Console.WriteLine($"This is the binary before the final permutation : {L}{R}");
            string one_last_step = L + R;
            string mahmoud = "";
            for (int i = 0; i < inverseFinalPermutation.Length; i++)
            {
                mahmoud += one_last_step[inverseFinalPermutation[i] - 1];
            }
            /*
             * 00000001 00100011 01000101 01100111 10001001 10101011 11001101 11101111
             * 00000001 00100011 01000101 01100111 10001001 10101011 11001101 11101111
             */
            Console.WriteLine($"This is the binary representaion of the plain text : {mahmoud}");
            while (mahmoud.Length > 0)
            {
                string temp = mahmoud.Substring(0, 4);
                PlainText += Convert.ToInt32(temp, 2).ToString("X");
                mahmoud = mahmoud.Substring(4);
            }
            Console.WriteLine($"This is the plain text : {PlainText}");
            return PlainText;
        }

        public override string Encrypt(string plainText, string key)
        {
            string cipher = "0x";
            string binary_key = HexToBinary(key);
            Console.WriteLine($"Binary Key: {binary_key}");
            string[] key_after_pc1 = new string[56];
            //we applied the step of key so we get the first 56 bit of the key and we then will apply pc-2 and then repeate the next steps 16 times
            string c = "";
            string d = "";
            // convert the plain text to binary representation
            string binary_plainText = HexToBinary(plainText);
            //Console.WriteLine($"Binary Plain text : {binary_plainText}");
            //apply the initial permutaion
            string plainText_after_initial_permutation = "";
            for (int i = 0; i < initialPermutaion.Length; i++)
            {
                plainText_after_initial_permutation += binary_plainText[initialPermutaion[i] - 1];
            }
            Console.WriteLine($"this is the plain text after initial permutation : {plainText_after_initial_permutation}");
            //after this we will split the the binary code into two halfs which are l and r
            string L = plainText_after_initial_permutation.Substring(0, plainText_after_initial_permutation.Length / 2);
            string R = plainText_after_initial_permutation.Substring(plainText_after_initial_permutation.Length / 2);
            Console.WriteLine($"This is the L : {L} and this is the R : {R}");
            for (int i = 0; i < permutedChoice_1.Length; i++)
            {
                //Console.WriteLine($"this is the {i} round and i have added {binary_key[permutedChoice_1[i]-1]} to this place the permuted choice is {permutedChoice_1[i]}");
                key_after_pc1[i] = binary_key[permutedChoice_1[i] - 1].ToString();
            }
            for (int i = 0; i < key_after_pc1.Length; i++)
            {
                if (i < 28)
                {
                    c += key_after_pc1[i];
                }
                else
                {
                    d += key_after_pc1[i];
                }
            }
            //Console.WriteLine("This is after the pc1\n");
            //for (int i = 0; i < key_after_pc1.Length; i++)
            //{
            //    Console.Write(key_after_pc1[i]);
            //}
            //Console.WriteLine($"\n{c}{d}");
            int[] shift = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            for (int i = 0; i < 16; i++)
            {
                string R_After_expantion_permutaion = "";
                for (int j = 0; j < expantionPermutation.Length; j++)
                {
                    //Console.WriteLine($"this is the value of the expantion permutaion {expantionPermutation[j]} and this is what i get from the R {R[expantionPermutation[j]-1]}");
                    R_After_expantion_permutaion += R[expantionPermutation[j] - 1];
                }
                //Console.WriteLine($"this is after expantion permutaion {R_After_expantion_permutaion}");
                int shifts = shift[i];
                for (int j = 0; j < shifts; j++)
                {
                    c = c.Substring(1) + c[0];
                    d = d.Substring(1) + d[0];
                }
                string key_after_shift = c + d;
                //Console.WriteLine($"this is the shifted key\n{c}\n{d}");
                //now lets apply the permutation
                string key_after_pc_2 = "";
                for (int j = 0; j < permutedChoice_2.Length; j++)
                {
                    key_after_pc_2 += key_after_shift[permutedChoice_2[j] - 1];
                }
                //so we have done generating the keys
                //Console.WriteLine($"This is the {i} key after the pc2 {key_after_pc_2}");
                //we now will start to xor the R after the expantion permutation with the key after pc 2
                //Console.WriteLine($"This is the length of the R {R_After_expantion_permutaion.Length} and this is the length of key after pc 2 {key_after_pc_2.Length}");
                string xor_of_R_after_expantion_and_key_after_pc_2 = "";
                for (int j = 0; j < key_after_pc_2.Length; j++)
                {
                    if (key_after_pc_2[j] == R_After_expantion_permutaion[j])
                    {
                        xor_of_R_after_expantion_and_key_after_pc_2 += "0";
                    }
                    else
                    {
                        xor_of_R_after_expantion_and_key_after_pc_2 += "1";
                    }
                }
                //Console.WriteLine($"This is the result of the first xor {xor_of_R_after_expantion_and_key_after_pc_2}");
                //now we will start to get the s-boxes result
                //Console.WriteLine(SBoxes[1, 1, 1]);
                string temp = xor_of_R_after_expantion_and_key_after_pc_2;
                string out_of_the_s_boxes = "";
                for (int j = 0; j < 8; j++)
                {

                    string sub = temp.Substring(0, 6);

                    temp = temp.Substring(6);
                    //Console.WriteLine($"This is the substring {sub}");
                    int row_number = Convert.ToInt32(sub[0] + sub[sub.Length - 1].ToString(), 2);
                    int column_number = Convert.ToInt32(sub.Substring(1, 4), 2);
                    //Console.WriteLine($"this is the binary for the column {sub.Substring(1, 4)} and this is the binary for the row number {sub[0] + sub[sub.Length - 1].ToString()}");
                    int decimal_representation = SBoxes[j, row_number, column_number];
                    //Console.WriteLine($"This is the decimal number {decimal_representation}");
                    out_of_the_s_boxes += Convert.ToString(decimal_representation, 2).PadLeft(4, '0');
                }
                //Console.WriteLine($"This is the output of the sboxes : {out_of_the_s_boxes}");
                //wee now apply the permutation function to the s-boxes
                string permutaion_of_s_boxes = "";
                for (int j = 0; j < permutationFunction.Length; j++)
                {
                    permutaion_of_s_boxes += out_of_the_s_boxes[permutationFunction[j] - 1];
                }
                //Console.WriteLine($"this is the permuatation after the s-boxes : {permutaion_of_s_boxes}");
                //Console.WriteLine($"This is the length of L : {L.Length} and this is the length of permutation of the s-boxes : {permutaion_of_s_boxes.Length}");
                //now xor the L and the permutation of the s boxes
                string xor_of_L_and_permutation_of_s_boxes = "";
                for (int j = 0; j < L.Length; j++)
                {
                    if (L[j] == permutaion_of_s_boxes[j])
                    {
                        xor_of_L_and_permutation_of_s_boxes += "0";
                    }
                    else
                    {
                        xor_of_L_and_permutation_of_s_boxes += "1";
                    }
                }
                //Console.WriteLine($"this is the output of the xor of the l and the permutation of the s-boxes : {xor_of_L_and_permutation_of_s_boxes}");
                L = R;
                R = xor_of_L_and_permutation_of_s_boxes;

                //Console.WriteLine($"This is the L : {L} this is the R : {R} for the {i} round");
            }
            //Console.WriteLine($"this is the final cipher binary : {R}{L}\n");
            string swaped_cipher = R + L;
            string inversePermutation = "";
            for (int j = 0; j < inverseFinalPermutation.Length; j++)
            {
                inversePermutation += swaped_cipher[inverseFinalPermutation[j] - 1];
            }
            while (inversePermutation.Length > 0)
            {
                string temp = inversePermutation.Substring(0, 4);
                cipher += Convert.ToInt32(temp, 2).ToString("X");
                inversePermutation = inversePermutation.Substring(4);
            }
            Console.WriteLine($"This is the final cipher : {cipher}");
            return cipher;
        }
        static string HexToBinary(string hexText)
        {
            string binaryText = "";
            if (hexText.StartsWith("0x"))
            {
                hexText = hexText.Substring(2);
            }
            foreach (char hexChar in hexText)
            {

                int value = Convert.ToInt32(hexChar.ToString(), 16);
                //change the value to binary and then pad left so that each char is represented by 4 bits
                binaryText += Convert.ToString(value, 2).PadLeft(4, '0');
            }
            return binaryText;
        }
        static readonly int[] permutedChoice_1 = {
        57, 49, 41, 33, 25, 17, 9,
        1, 58, 50, 42, 34, 26, 18,
        10, 2, 59, 51, 43, 35, 27,
        19, 11, 3, 60, 52, 44, 36,

        63, 55, 47, 39, 31, 23, 15,
        7, 62, 54, 46, 38, 30, 22,
        14, 6, 61, 53, 45, 37, 29,
        21, 13, 5, 28, 20, 12, 4
    };
        static readonly int[] permutedChoice_2 = {
        14, 17, 11, 24, 1, 5,
        3, 28, 15, 6, 21, 10,
        23, 19, 12, 4, 26, 8,
        16, 7, 27, 20, 13, 2,
        41, 52, 31, 37, 47, 55,
        30, 40, 51, 45, 33, 48,
        44, 49, 39, 56, 34, 53,
        46, 42, 50, 36, 29, 32,
    };
        static readonly int[] expantionPermutation =
        {
        32, 1, 2, 3, 4, 5,
        4, 5, 6, 7, 8, 9,
        8, 9, 10, 11, 12, 13,
        12, 13, 14, 15, 16, 17,
        16, 17, 18, 19, 20, 21,
        20, 21, 22, 23, 24, 25,
        24, 25, 26, 27, 28, 29,
        28, 29, 30, 31, 32, 1
    };
        static readonly int[] initialPermutaion =
        {
        58, 50, 42, 34, 26, 18, 10, 2,
        60, 52, 44, 36, 28, 20, 12, 4,
        62, 54, 46, 38, 30, 22, 14, 6,
        64, 56, 48, 40, 32, 24, 16, 8,
        57, 49, 41, 33, 25, 17, 9, 1,
        59, 51, 43, 35, 27, 19, 11, 3,
        61, 53, 45, 37, 29, 21, 13, 5,
        63, 55, 47, 39, 31, 23, 15, 7
    };
        static readonly int[] permutationFunction =
        {
        16, 7, 20, 21,
        29, 12, 28, 17,
        1, 15, 23, 26,
        5, 18, 31, 10,
        2, 8, 24, 14,
        32, 27, 3, 9,
        19, 13, 30, 6,
        22, 11, 4, 25
    };
        //These are the S-boxes
        static readonly int[,,] SBoxes =
    {
    {
        { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
        { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
        { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
        { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
    },
    {
        { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
        { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
        { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
        { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
    },
    {
        { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
        { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
        { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
        { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
    },
    {
        { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
        { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
        { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
        { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
    },
    {
        { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
        { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
        { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
        { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
    },
    {
        { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
        { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
        { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
        { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
    },
    {
        { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
        { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
        { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
        { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
    },
    {
        { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
        { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
        { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
        { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
    }
};
        static readonly int[] inverseFinalPermutation =
        {
        40, 8, 48, 16, 56, 24, 64, 32,
        39, 7, 47, 15, 55, 23, 63, 31,
        38, 6, 46, 14, 54, 22, 62, 30,
        37, 5, 45, 13, 53, 21, 61, 29,
        36, 4, 44, 12, 52, 20, 60, 28,
        35, 3, 43, 11, 51, 19, 59, 27,
        34, 2, 42, 10, 50, 18, 58, 26,
        33, 1, 41, 9, 49, 17, 57, 25
    };
    }

}
