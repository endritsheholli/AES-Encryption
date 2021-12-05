using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace AES_Encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter text that needs to be encrypted or decrypted:");
            string data = Console.ReadLine();
            Console.WriteLine("Do you want to encrypt or decrypt? (E/D)");
            string action = Console.ReadLine().ToLower();
            Console.WriteLine("Do you want to use your own Key and IV? (Y/N)");
            string response = Console.ReadLine().ToLower();

            if (response == "y")
            {
                Console.WriteLine("Give your Key:");
                string key = Console.ReadLine();
                Console.WriteLine("Give your IV:");
                string iv = Console.ReadLine();
                Dictionary<String, String> parameters = new Dictionary<String, String> { { "Key", key }, { "IV", iv } };
                EncryptAesManaged(data, action, parameters);
            }
            else
            {
                EncryptAesManaged(data, action);
            }
            Console.ReadLine();
        }
        static void EncryptAesManaged(String data, String action, Dictionary<String, String> kwargs = null)
        {
            // Check arguments.
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("data");
            if (action == null || action.Length <= 0)
                throw new ArgumentNullException("action");

            try
            {
                // Create a new instance of the Aes class.
                // This generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption.   
                using (Aes myAes = Aes.Create())
                {
                    // If key and initialization vector are given, they should be used to configure Aes key and IV.
                    if (kwargs != null)
                    {
                        string key = kwargs["Key"];
                        string iv = kwargs["IV"];

                        if (key.Length != 44 && iv.Length != 24)
                        {
                            // Create e new insctance of HashAlgorithm class which implemets MD5 hash algorithm.
                            HashAlgorithm hash = MD5.Create();

                            // ComputeHash method takes a byte array as an input and returns a hash in the form of byte array of 128 bits.
                            // No matter how big the input data is, the hash will always be 128 bits. 
                            myAes.Key = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
                            myAes.IV = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(iv));
                        }
                        else 
                        {
                            myAes.Key = Convert.FromBase64String(key);
                            myAes.IV = Convert.FromBase64String(iv);

                        }
                    }

                    Console.WriteLine($"Encryption key: {Convert.ToBase64String(myAes.Key)}");
                    Console.WriteLine($"Encryption IV: {Convert.ToBase64String(myAes.IV)}");

                    if (action == "e")
                    {
                        // Encrypt string    
                        byte[] encrypted = Encrypt(data, myAes.Key, myAes.IV);
                        // Print encrypted string    
                        Console.WriteLine($"Encrypted data:{Convert.ToBase64String(encrypted)}");

                    }
                    else
                    { 
                        // Decrypt string
                        string decrypted = Decrypt(Convert.FromBase64String(data), myAes.Key, myAes.IV);
                        // Print decrypted string  
                        Console.WriteLine($"Decrypted data: {decrypted}");
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            Console.ReadKey();
        }

        static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Use a stream that links data streams to cryptographic transformations.
                    // Create a CryptoStream, pass it the msEncrypt, and encrypt it with the Aes class encryptor. 
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        // Create a StreamWriter for easy writing to the stream.
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return encrypted data    
            return encrypted;
        }

        static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    // Use a stream that links data streams to cryptographic transformations.
                    // Create a CryptoStream, pass it the msDecrypt, and dencrypt it with the Aes class decryptor. 
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        // Create a StreamRader for easy writing to the stream.
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            // Return decrypted data  
            return plaintext;
        }
    }
}
