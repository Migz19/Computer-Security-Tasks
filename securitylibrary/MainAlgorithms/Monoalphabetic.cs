using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        int GetCharIndex(char l, string t)
        {
            int n = t.Length;
            int res = -1;
            for (int i = 0; i < n; i++)
            {
                if (l == t[i])
                {res = i; break;}
            }
            return res;
        }
        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            StringBuilder key = new StringBuilder(26);
            bool[] usedChars = new bool[26];
            for (int i = 0; i < 26; i++)
            {
                char letter = (char)(i + 'a');
                int letterIndex = GetCharIndex(letter, plainText);
                if (letterIndex == -1)
                {
                    key.Append(' ');
                }
                else
                {
                    char cipherLetter = cipherText[letterIndex];
                    key.Append(cipherLetter);
                    usedChars[cipherLetter - 'a'] = true;
                }

            }
            for (int i = 0, c = 0; i < 26; i++)
            {
                if (key[i] == ' ')
                {
                    while (usedChars[c])
                    {
                        ++c;
                    }
                    usedChars[c] = true;
                    char fillerLetter = (char)(c + 'a');
                    key[i] = fillerLetter;
                }
            }
            return key.ToString();
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            StringBuilder plainText = new StringBuilder();
            foreach (char letter in cipherText)
            {
                int letterKeyIndex = GetCharIndex(letter, key) + 'a';
                char originalLetter = (char)letterKeyIndex;
                plainText.Append(originalLetter);
            }
            return plainText.ToString();
        }

        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToLower();

            StringBuilder result = new StringBuilder();
            foreach (char letter in plainText)
            {
                int keyIndex = letter - 'a';
                result.Append(key[keyIndex]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            List<char> lettersList = new List<char>
            {
                'E', 'T', 'A', 'O', 'I', 'N', 'S', 'R', 'H', 'L', 'D',
                'C', 'U', 'M', 'F', 'P', 'G', 'W', 'Y', 'B', 'V', 'K',
                'X', 'J', 'Q', 'Z'
            };

            Dictionary<char, int> lettersFreq = new Dictionary<char, int>();
            foreach (char letter in cipher)
            {
                if (lettersFreq.ContainsKey(letter) == false)
                {
                    lettersFreq.Add(letter, 1);
                }
                else
                {
                    lettersFreq[letter]++;
                }

            }

            lettersFreq = lettersFreq.OrderBy(p => -p.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            Dictionary<char, char> LettersMap = new Dictionary<char, char>();
            int n = lettersFreq.Count;
            for (int i = 0; i < 26; i++)
            {
                char key = lettersFreq.ToArray()[i].Key;
                char val = lettersList[i];
                LettersMap.Add(key, val);
            }

            StringBuilder plain = new StringBuilder();

            foreach (char letter in cipher)
            {
                char targetLetter = LettersMap[letter];
                plain.Append(targetLetter);
            }
            return plain.ToString().ToLower();
        }
    }
}
