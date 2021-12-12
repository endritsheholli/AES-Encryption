using AES_Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public class main
    {
        public const int BLOCK_SIZE = (128 / 8);
        public const int KEY_SIZE = (128 / 8);
        public const int ROUND_NO = 10;

        static void Main(string[] args)
        {
            byte[] pass = Encoding.ASCII.GetBytes("test");
            //Encrypt(pass, ref pass);
            AES128 aes128 = new AES128();
            aes128.Encrypt(pass, ref pass);
        }

    }
}