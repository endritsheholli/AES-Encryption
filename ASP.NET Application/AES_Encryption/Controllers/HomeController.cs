using AES_Encryption.Models;
using AES_Encryption.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace AES_Encryption.Controllers
{
    public class HomeController : Controller
    {
        EncryptAesService encryptAesService = new EncryptAesService();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Encryption()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Encryption(String plaintext, String key, String iv)
        {
            AESModel encryptModel = new AESModel();
            if (plaintext != "")
            {
                List<String> results = new List<string>();
                results = encryptAesService.EncryptAesManaged(plaintext, "e", key, iv);

                encryptModel.plaintext = plaintext;
                encryptModel.ciphertext = results[2];
                encryptModel.key = results[0];
                encryptModel.iv = results[1];
                encryptModel.plainError = "";
            }
            else
            {
                encryptModel.plainError = "Plaintext field should not be empty!";
            }
            return View(encryptModel);
        }

        public ActionResult Decryption()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Decryption(String ciphertext, String key, String iv)
        {
            AESModel encryptModel = new AESModel();
            encryptModel.ciphertext = ciphertext;
            encryptModel.key = key;
            if (ciphertext == "")
            {
                encryptModel.cipherError = "Ciphertext field should not be empty!";
            }
            else if (key == "")
            {
                encryptModel.keyError = "Key field should not be empty!";
            }
            else if (iv == "")
            {
                encryptModel.ivError = "IV field should not be empty!";
            }
            else
            {
                try
                {
                    List<String> results = encryptAesService.EncryptAesManaged(ciphertext, "d", key, iv);
                    encryptModel.plaintext = results[2];
                    encryptModel.key = results[0];
                    encryptModel.iv = results[1];
                    encryptModel.cipherError = "";
                    encryptModel.keyError = "";
                    encryptModel.ivError = "";
                    encryptModel.error = "";
                }
                catch (Exception)
                {
                    encryptModel.error = "Invalid Key or IV!";
                    encryptModel.key = "";
                }

            }
            return View(encryptModel);
        }

    }

}