using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        //static string all_letters = "abcdefghijklmnopqrstuvwxyz";

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            if (cipherText.Length % 2 == 1 && cipherText[cipherText.Length - 1] == 'x')
            {
                cipherText = cipherText.Substring(0, cipherText.Length - 1);
            }
            string reversed_all_letters = new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().Reverse().ToArray()); // Define `all_letters`
            string reversed_key = new string(key.Reverse().ToArray());
            reversed_key = reversed_key.ToLower();
            //reversed_key = reversed_key.TrimEnd();
            string plaintextt = "";
            bool checkij = false;
            //Console.WriteLine(reversed_key);
            // Initialize matrix
            string[][] matrix = new string[5][];
            for (int i = 0; i < 5; i++)
            {
                matrix[i] = new string[5];
            }
            //building up the matrix
            string matrix_alphabet = "";
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.WriteLine($"iam now in the {i} row and in the {j} column");
                    //Console.WriteLine(string.IsNullOrEmpty(reversed_key));
                    if (!string.IsNullOrEmpty(reversed_key))
                    {
                        //Console.WriteLine("iam in the first condition");
                        while (reversed_key.Length > 0)
                        {
                            //Console.WriteLine("iam in the loop");
                            if ((matrix_alphabet.Contains(reversed_key[reversed_key.Length - 1])) || reversed_key[reversed_key.Length - 1] == ' ')
                            {
                                reversed_key = reversed_key.Substring(0, reversed_key.Length - 1);
                                //Console.WriteLine("i have removed the last character");
                            }
                            else
                            {
                                break;
                            }

                        }
                        if (reversed_key.Length > 0 && (reversed_key[reversed_key.Length - 1] == 'i' || reversed_key[reversed_key.Length - 1] == 'j') && !checkij)
                        {
                            //Console.WriteLine("i have now inserted the ij");
                            matrix_alphabet += "ij";
                            matrix[i][j] = "ij";
                            checkij = true;
                            Console.WriteLine("i have written the ij");
                            continue;
                        }
                        if (reversed_key.Length > 0)
                        {
                            matrix[i][j] = reversed_key[reversed_key.Length - 1].ToString();
                            //Console.WriteLine("i have appended the " + reversed_key[^1] + " char");
                            matrix_alphabet += reversed_key[reversed_key.Length - 1];
                            reversed_key = reversed_key.Substring(0, reversed_key.Length - 1);
                        }
                    }
                    if (string.IsNullOrEmpty(matrix[i][j]))
                    {
                        while (reversed_all_letters.Length > 0 && (matrix_alphabet.Contains(reversed_all_letters[reversed_all_letters.Length - 1]) || string.IsNullOrEmpty(reversed_all_letters)))
                        {
                            reversed_all_letters = reversed_all_letters.Substring(0, reversed_all_letters.Length - 1);
                        }
                        if (reversed_all_letters.Length > 0 && (reversed_all_letters[reversed_all_letters.Length - 1] == 'i' || reversed_all_letters[reversed_all_letters.Length - 1] == 'j') && !checkij)
                        {
                            Console.WriteLine("i have inserted the ij from the lower condition");
                            matrix_alphabet += "ij";
                            matrix[i][j] = "ij";
                            checkij = true;
                            continue;
                        }
                        if (reversed_all_letters.Length > 0)
                        {
                            //Console.WriteLine($"i have now inserted the {reversed_all_letters[^1]} charachter from the lower condition");
                            matrix[i][j] = reversed_all_letters[reversed_all_letters.Length - 1].ToString();
                            matrix_alphabet += reversed_all_letters[reversed_all_letters.Length - 1];
                            reversed_all_letters = reversed_all_letters.Substring(0, reversed_all_letters.Length - 1);
                        }
                    }
                }
            }

            //matrix done

            //dealing with the plan text

            string reversed_cypher_text = new string(cipherText.Reverse().ToArray());
            while (!string.IsNullOrEmpty(reversed_cypher_text))
            {
                charct_index char_1 = new charct_index();
                charct_index char_2 = new charct_index();
                char_1.character = reversed_cypher_text[reversed_cypher_text.Length - 1].ToString();
                reversed_cypher_text = reversed_cypher_text.Substring(0, reversed_cypher_text.Length - 1);
                if (reversed_cypher_text.Length == 0 || reversed_cypher_text[reversed_cypher_text.Length - 1] == char_1.character[char_1.character.Length - 1])
                {
                    char_2.character = "x";
                }
                else
                {
                    char_2.character = reversed_cypher_text[reversed_cypher_text.Length - 1].ToString();
                    reversed_cypher_text = reversed_cypher_text.Substring(0, reversed_cypher_text.Length - 1);
                }
                //then this is the loop on the matrix to find the values in it
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (matrix[i][j][0].ToString() == char_1.character)
                        {
                            char_1.row = i;
                            char_1.column = j;
                        }
                        if (matrix[i][j][0].ToString() == char_2.character)
                        {
                            char_2.row = i;
                            char_2.column = j;
                        }
                        if (matrix[i][j].Length == 2)
                        {
                            if (matrix[i][j][0].ToString() == char_1.character)
                            {
                                char_1.row = i;
                                char_1.column = j;
                            }
                            if (matrix[i][j][1].ToString() == char_1.character)
                            {
                                char_1.row = i;
                                char_1.column = j;
                            }
                            if (matrix[i][j][0].ToString() == char_2.character)
                            {
                                char_2.row = i;
                                char_2.column = j;
                            }
                            if (matrix[i][j][1].ToString() == char_2.character)
                            {
                                char_2.row = i;
                                char_2.column = j;
                            }
                        }
                    }
                }
                //first condition if they are in the same row
                if (char_1.row == char_2.row)
                {
                    plaintextt += matrix[char_1.row][(char_1.column - 1 + 5) % 5][0];
                    plaintextt += matrix[char_1.row][(char_2.column - 1 + 5) % 5][0];
                }
                //second condition if they are in the same column
                else if (char_1.column == char_2.column)
                {
                    plaintextt += matrix[(char_1.row - 1 + 5) % 5][char_1.column][0];
                    plaintextt += matrix[(char_2.row - 1 + 5) % 5][char_2.column][0];
                }
                else
                {
                    plaintextt += matrix[char_1.row][char_2.column][0];
                    plaintextt += matrix[char_2.row][char_1.column][0];
                }
            }
            while (true)
            {
                if (plaintextt[plaintextt.Length - 1] == 'x')
                {
                    plaintextt = plaintextt.Substring(0, plaintextt.Length - 1);
                }
                else
                {
                    break;
                }
            }
            string temp_text = "";
            plaintextt = plaintextt.ToLower();
            int len = plaintextt.Length;

            for(int i = 0; i < len; i++)
            {
                if(i % 2 == 0)
                {
                    temp_text += plaintextt[i];
                }
                else
                {
                    if (plaintextt[i] == 'x')
                    {
                        if (i == len - 1)
                        {
                            continue;
                        }else if (plaintextt[i-1] == plaintextt[i+1]){
                            continue;
                        }
                        else
                        {
                            temp_text+= plaintextt[i];
                        }
                    }
                    else
                    {
                        temp_text += plaintextt[i];
                    }
                }
            }
            //Console.WriteLine("decryption matrix");
            //Print Matrix
            //for (int i = 0; i < 5; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        Console.Write(matrix[i][j] + " ");
            //    }
            //    Console.WriteLine();
            //}
            return temp_text;
        }

        public string Encrypt(string plainText, string key)
        {
            string reversed_all_letters = new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().Reverse().ToArray()); // Define `all_letters`
            string reversed_key = new string(key.Reverse().ToArray());
            reversed_key = reversed_key.ToLower();
            //reversed_key = reversed_key.TrimEnd();
            string cypherText = "";
            bool checkij = false;
            //Console.WriteLine(reversed_key);
            // Initialize matrix
            string[][] matrix = new string[5][];
            for (int i = 0; i < 5; i++)
            {
                matrix[i] = new string[5];
            }
            //building up the matrix
            string matrix_alphabet = "";
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    //Console.WriteLine($"iam now in the {i} row and in the {j} column");
                    //Console.WriteLine(string.IsNullOrEmpty(reversed_key));
                    if (!string.IsNullOrEmpty(reversed_key))
                    {
                        //Console.WriteLine("iam in the first condition");
                        while (reversed_key.Length > 0)
                        {
                            //Console.WriteLine("iam in the loop");
                            if ((matrix_alphabet.Contains(reversed_key[reversed_key.Length - 1])) || reversed_key[reversed_key.Length - 1] == ' ')
                            {
                                reversed_key = reversed_key.Substring(0, reversed_key.Length - 1);
                                //Console.WriteLine("i have removed the last character");
                            }
                            else
                            {
                                break;
                            }

                        }
                        if (reversed_key.Length > 0 && (reversed_key[reversed_key.Length - 1] == 'i' || reversed_key[reversed_key.Length - 1] == 'j') && !checkij)
                        {
                            //Console.WriteLine("i have now inserted the ij");
                            matrix_alphabet += "ij";
                            matrix[i][j] = "ij";
                            checkij = true;
                            continue;
                        }
                        if (reversed_key.Length > 0)
                        {
                            matrix[i][j] = reversed_key[reversed_key.Length - 1].ToString();
                            //Console.WriteLine("i have appended the " + reversed_key[^1] + " char");
                            matrix_alphabet += reversed_key[reversed_key.Length - 1];
                            reversed_key = reversed_key.Substring(0, reversed_key.Length - 1);
                        }
                    }
                    if (string.IsNullOrEmpty(matrix[i][j]))
                    {
                        while (reversed_all_letters.Length > 0 && (matrix_alphabet.Contains(reversed_all_letters[reversed_all_letters.Length - 1]) || string.IsNullOrEmpty(reversed_all_letters)))
                        {
                            reversed_all_letters = reversed_all_letters.Substring(0, reversed_all_letters.Length - 1);
                        }
                        if (reversed_all_letters.Length > 0 && (reversed_all_letters[reversed_all_letters.Length - 1] == 'i' || reversed_all_letters[reversed_all_letters.Length - 1] == 'j') && !checkij)
                        {
                            //Console.WriteLine("i have inserted the ij from the lower condition");
                            matrix_alphabet += "ij";
                            matrix[i][j] = "ij";
                            checkij = true;
                            continue;
                        }
                        if (reversed_all_letters.Length > 0)
                        {
                            //Console.WriteLine($"i have now inserted the {reversed_all_letters[^1]} charachter from the lower condition");
                            matrix[i][j] = reversed_all_letters[reversed_all_letters.Length - 1].ToString();
                            matrix_alphabet += reversed_all_letters[reversed_all_letters.Length - 1];
                            reversed_all_letters = reversed_all_letters.Substring(0, reversed_all_letters.Length - 1);
                        }
                    }
                }
            }

            //matrix done

            //dealing with the plan text
            string reversed_plain_text = new string(plainText.Reverse().ToArray());
            while (!string.IsNullOrEmpty(reversed_plain_text))
            {
                charct_index char_1 = new charct_index();
                charct_index char_2 = new charct_index();
                char_1.character = reversed_plain_text[reversed_plain_text.Length - 1].ToString();
                reversed_plain_text = reversed_plain_text.Substring(0, reversed_plain_text.Length - 1);
                if (reversed_plain_text.Length == 0 || reversed_plain_text[reversed_plain_text.Length - 1] == char_1.character[char_1.character.Length - 1])
                {
                    char_2.character = "x";
                }
                else
                {
                    char_2.character = reversed_plain_text[reversed_plain_text.Length - 1].ToString();
                    reversed_plain_text = reversed_plain_text.Substring(0, reversed_plain_text.Length - 1);
                }
                //then this is the loop on the matrix to find the values in it
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (matrix[i][j][0].ToString() == char_1.character)
                        {
                            char_1.row = i;
                            char_1.column = j;
                        }
                        if (matrix[i][j][0].ToString() == char_2.character)
                        {
                            char_2.row = i;
                            char_2.column = j;
                        }
                        if (matrix[i][j].Length == 2)
                        {
                            if (matrix[i][j][0].ToString() == char_1.character)
                            {
                                char_1.row = i;
                                char_1.column = j;
                            }
                            if (matrix[i][j][1].ToString() == char_1.character)
                            {
                                char_1.row = i;
                                char_1.column = j;
                            }
                            if (matrix[i][j][0].ToString() == char_2.character)
                            {
                                char_2.row = i;
                                char_2.column = j;
                            }
                            if (matrix[i][j][1].ToString() == char_2.character)
                            {
                                char_2.row = i;
                                char_2.column = j;
                            }
                        }
                    }
                }
                //first condition if they are in the same row
                if (char_1.row == char_2.row)
                {
                    cypherText += matrix[char_1.row][(char_1.column + 1) % 5][0];
                    cypherText += matrix[char_1.row][(char_2.column + 1) % 5][0];
                }
                //second condition if they are in the same column
                else if (char_1.column == char_2.column)
                {
                    cypherText += matrix[(char_1.row + 1) % 5][char_1.column][0];
                    cypherText += matrix[(char_2.row + 1) % 5][char_2.column][0];
                }
                else
                {
                    cypherText += matrix[char_1.row][char_2.column][0];
                    cypherText += matrix[char_2.row][char_1.column][0];
                }
            }
            // Print Matrix
            //for (int i = 0; i < 5; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        Console.Write(matrix[i][j] + " ");
            //    }
            //    Console.WriteLine();
            //}
            return cypherText;
        }
    }
}

struct charct_index
{
    public string character;
    public int row;
    public int column;
}
