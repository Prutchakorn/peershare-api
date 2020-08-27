using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using PeerShareV2.Models;
using PeerShareV2.Data;

namespace PeerShareV2.Providers
{
    public class SecurityProvider : ISecurityProvider
    {
        protected String crypticKey;
        protected Rfc2898DeriveBytes pdb;
        protected Aes encryptor;

        public SecurityProvider()
        {
            crypticKey = "asodofpojpeajg[b-[a6sa5dv68shr59yer893";
            pdb = new Rfc2898DeriveBytes(crypticKey, new byte[]
            { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor = Aes.Create();
        }
        
        public String GetEncrypt(String clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            var encrypted = "";
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(memoryStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                encrypted = System.Convert.ToBase64String(memoryStream.ToArray());
            }
            return encrypted;
        }

        public String GetDecrypt(String encrypted)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            var clearText = "";
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                    cs.Close();
                }
                clearText = Encoding.Unicode.GetString(ms.ToArray());
            }
            return clearText;
        }
    }
}