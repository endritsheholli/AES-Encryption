using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace AES_Encryption.Services
{
    public class EncryptAesService
    {
        EncryptService encryptService = new EncryptService();
        DecryptService decyptService = new DecryptService();

        public EncryptAesService()
        {
        }

        public List<String> EncryptAesManaged(String data, String action, String key, String iv)
        {
            List<String> key_iv = new List<String>();
            try
            {
                using (Aes myAes = Aes.Create())
                {
                    HashAlgorithm hashmd5 = MD5.Create();
                    HashAlgorithm hashsha256 = SHA256.Create();

                    // encrypt
                    if (action == "e")
                    {
                        // Only plaintext is given
                        if (key == "" && iv == "")
                        {
                            key_iv.Add(Convert.ToBase64String(myAes.Key));
                            key_iv.Add(Convert.ToBase64String(myAes.IV));
                            myAes.Key = hashsha256.ComputeHash(myAes.Key);
                            myAes.IV = hashmd5.ComputeHash(myAes.IV);
                        }
                        // Plaintext and encryption key are given
                        else if (key != "" && iv == "")
                        {
                            key_iv.Add(key);
                            key_iv.Add(Convert.ToBase64String(myAes.IV));
                            myAes.Key = hashsha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
                            myAes.IV = hashmd5.ComputeHash(myAes.IV);
                        }
                        // Plaintext, key and initialization vector are given
                        else
                        {
                            key_iv.Add(key);
                            key_iv.Add(iv);
                            myAes.Key = hashsha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
                            myAes.IV = hashmd5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(iv));
                        }

                        byte[] encrypted = encryptService.Encrypt(data, myAes.Key, myAes.IV);
                        key_iv.Add(Convert.ToBase64String(encrypted));
                    }
                    // decrypt 
                    else
                    {
                        key_iv.Add(key);
                        key_iv.Add(iv);

                        try
                        {
                            myAes.Key = hashsha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
                            myAes.IV = hashmd5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(iv));
                            string decrypted = decyptService.Decrypt(Convert.FromBase64String(data), myAes.Key, myAes.IV);
                            key_iv.Add(decrypted);
                        }
                        catch
                        {
                            try
                            {
                                myAes.Key = hashsha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
                                myAes.IV = hashmd5.ComputeHash(Convert.FromBase64String(iv));
                                string decrypted = decyptService.Decrypt(Convert.FromBase64String(data), myAes.Key, myAes.IV);
                                key_iv.Add(decrypted);
                            }
                            catch
                            {
                                myAes.Key = hashsha256.ComputeHash(Convert.FromBase64String(key));
                                myAes.IV = hashmd5.ComputeHash(Convert.FromBase64String(iv));
                                string decrypted = decyptService.Decrypt(Convert.FromBase64String(data), myAes.Key, myAes.IV);
                                key_iv.Add(decrypted);
                            }
                        }
                    }
                }
            }
            catch (Exception exp) { }
            return key_iv;
        }

    }


}
