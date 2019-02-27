using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KryptoZaba
{
    /// <summary>
    /// Provides main logic for cryptology operations.
    /// </summary>
    static class CryptoFrog
    {
        /// <summary>
        /// Set of letters, digits and signs used by <see cref="CryptoFrog"/> to perform encryption task.
        /// </summary>
        public static readonly char[] alphabet = { '`', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '=', 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', '[', ']', '\\', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';', '\'', 'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.', '/', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', '{', '}', '|', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', ':', '"', 'Z', 'X', 'C', 'V', 'B', 'N', 'M', '<', '>', '?', 'ę', 'ó', 'ą', 'ś','ł','ż', 'ź', 'ć', 'ń', ' ', '\r','\n','Ę', 'Ó', 'Ą', 'Ś', 'Ł', 'Ż', 'Ź', 'Ć', 'Ń'};

        /// <summary>
        /// Translates a string into Key for Caesar cipher.
        /// </summary>
        /// <param name="Key">Text to translate into key (number)</param>
        /// <returns>Key - number</returns>
        public static int TranslateKey(string Key)
        {
            int keyNumber;

            //Parsuje Key do zmiennej keyNumber i jesli staloby sie tak, ze metoda zwroci false do wykonuje kod w klamrach
            if (!int.TryParse(Key, out keyNumber))
            {
                foreach (char znak in Key)
                {
                    keyNumber += Array.IndexOf(alphabet, znak);
                }
            }
            return keyNumber;
        }

        /// <summary>
        /// Encrypts given text with Caesar cipher.
        /// </summary>
        /// <param name="text">Plain text for encryption</param>
        /// <param name="Key">Key use to encrypt</param>
        /// <returns>cryptogram</returns>
        public static string CaesarEncrypt(string text, string Key)
        {
            int alphabetNumber = alphabet.Length;
            int keyNumber = TranslateKey(Key);
            int oldSignNumber;
            int newSignNumber;
            string cryptogram = "";
   
            foreach (char znak in text)
            {
                //pobierz nr znaku
                oldSignNumber = Array.IndexOf(alphabet, znak);
                //dodaj do numeru klucz
                newSignNumber = oldSignNumber + keyNumber;
                //wylicz modulo
                newSignNumber = (newSignNumber % alphabetNumber);
                //podstaw znak z nowym numerem
                cryptogram += alphabet[newSignNumber].ToString();
            }
            return cryptogram;
        }

        /// <summary>
        /// Decrypts given cryptogram into a plain text.
        /// </summary>
        /// <param name="cryptogram">An encrypted text</param>
        /// <param name="Key">Key use to decrypt </param>
        /// <returns>Plain text</returns>
        public static string CaesarDecrypt(string cryptogram, string Key)
        {
            int alphabetNumber = alphabet.Length;
            int keyNumber = TranslateKey(Key);
            int oldSignNumber;
            int newSignNumber;
            string plainText = "";

            foreach (char znak in cryptogram)
            {
                //Sprawdzic nr znaku w kryptogramie
                oldSignNumber = Array.IndexOf(alphabet, znak);
                //Odjac od numeru klucz
                newSignNumber = oldSignNumber - keyNumber;
                //Modulo alfabet
                newSignNumber = (newSignNumber % alphabetNumber + alphabetNumber) % alphabetNumber;
                //Podstawic znak o wyliczonym numerze
                plainText += alphabet[newSignNumber];
                //Dopisac do odp
            }
            return plainText;
        }

        /// <summary>
        /// Encrypts given text with Vigenere cipher.
        /// </summary>
        /// <param name="text">Text to encrypt</param>
        /// <param name="Key">Key use to encryption</param>
        /// <returns>cyrptogram</returns>
        public static string VigenereEncrypt(string text, string Key)
        {
            int pozycjaKlucza = 0;
            string cryptogram = "";

            foreach (char znak in text)
            {
                if (pozycjaKlucza > Key.Length-1)
                    pozycjaKlucza = 0;
                cryptogram += CaesarEncrypt(znak.ToString(), Key[pozycjaKlucza].ToString());
                pozycjaKlucza++;
                 
            }
            return cryptogram;
        }

        /// <summary>
        /// Decrypts given cryptogtam encrypted with a Vigenere cipher.
        /// </summary>
        /// <param name="cryptogtam">Cryptogtam to decryption</param>
        /// <param name="Key">Key used for encryption</param>
        /// <returns>plain text</returns>
        public static string VigenereDecrypt(string cryptogtam, string Key)
        {
            int pozycjaKlucza = 0;
            string plainText = "";

            foreach (char znak in cryptogtam)
            {
                if (pozycjaKlucza > Key.Length-1)
                    pozycjaKlucza = 0;
                plainText += CaesarDecrypt(znak.ToString(), Key[pozycjaKlucza].ToString());
                pozycjaKlucza++;
                
            }
            return plainText;
        }

        /// <summary>
        /// Tries to break Caesar cipher by bruteforce algorithm using a given word.
        /// </summary>
        /// <param name="cryptogram">Cryptogram to break</param>
        /// <param name="word">A word which is suspected to be a part of cryptogram.</param>
        /// <returns>List of possible texts</returns>
        public static List<string> BreakCaesar(string cryptogram, string word )
        {
            List<string> wyniki = new List<string>();
            string decryptedText = "";

            for (int literka = 0; literka < alphabet.Length; literka++)
            {
                decryptedText = CaesarDecrypt(cryptogram, literka.ToString());
                if (decryptedText.Contains(word))
                    wyniki.Add(decryptedText);
            }
            return wyniki;
        }

        /// <summary>
        /// Tries to break a Vigenere cipher with bruteforce recursive algorithm.
        /// </summary>
        /// <param name="depth">The quantity of letters algorithm will check. DO NOT SET THIS PARAMETER GREATER THAN 6!</param>
        /// <param name="literki">Just pass and empty list of strings</param>
        /// <param name="cryptogtam">Cryptogram to break</param>
        /// <param name="word">A word which is suspected to be a part of cryptogram.</param>
        /// <param name="wyniki">List to which method will return it results</param>
        public static void VigenereResurce(BackgroundWorker bw, int depth, List<string> literki, string cryptogtam, string word, ref List<string> wyniki)
        {
            if (bw.CancellationPending)
                return;

            if (depth <= 0)
            {
                    string wynik = "";
                    string key = "";

                    foreach (string literka in literki)
                    {
                        key += literka;
                    }

                    wynik = VigenereDecrypt(cryptogtam, key);
                    if (wynik.Contains(word))
                        wyniki.Add(wynik);
             }
                else
                {

                    for (int i = 0; i < alphabet.Length; i++)
                    {
                        List<string> noweLiterki = new List<string>(literki);
                        noweLiterki.Add(alphabet[i].ToString());
                        VigenereResurce(bw, depth - 1, noweLiterki, cryptogtam, word, ref wyniki);
                        if (depth < 0 || literki.Count > 4)
                            throw new StackOverflowException();
                    }

                }
            

        }


    }
}
