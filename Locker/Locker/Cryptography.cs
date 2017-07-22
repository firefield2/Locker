using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locker
{
    class Cryptography
    {
        public static void EncryptFile(string password, string entryFile, string outputFile)
        {
            byte[] bytesToBeEncrypted = File.ReadAllBytes(entryFile);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            File.WriteAllBytes(outputFile, bytesEncrypted);
        }
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            byte[] saltBytes = new byte[] { 0x0, 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0xF1, 0xF0, 0xEE, 0x21, 0x22, 0x45 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Padding = PaddingMode.Zeros;
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static void DecryptFile(string password, string entryFile, string outputFile)
        {
            byte[] bytesToBeDecrypted = File.ReadAllBytes(entryFile);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            File.WriteAllBytes(outputFile, bytesDecrypted);
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 0x0, 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0xF1, 0xF0, 0xEE, 0x21, 0x22, 0x45 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Padding = PaddingMode.Zeros;
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        //public static void CryptFile(string password, string entryFile, string outputFile, bool encrypt)
        //{
        //    using (FileStream input = new FileStream(entryFile, FileMode.Open, FileAccess.Read))
        //    {
        //        using (FileStream output = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
        //        {

        //            CryptStream(password, input, output, encrypt);
        //        }
        //    }
        //}

        //public static void CryptStream(string password, Stream input, Stream output, bool encrypt)
        //{
        //    AesCryptoServiceProvider aes_provider = new AesCryptoServiceProvider();

        //    int keyBits = 0;
        //    for (int i = 1024; i > 1; i--)
        //    {
        //        if (aes_provider.ValidKeySize(i))
        //        {
        //            keyBits = i;
        //            break;
        //        }
        //    }
        //    //Console.WriteLine("D?ugość Klucza: " + keyBits);
        //    int blockSize = aes_provider.BlockSize;

        //    byte[] code = { 0x0, 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0xF1, 0xF0, 0xEE, 0x21, 0x22, 0x45 };
        //    GenerateEncryprionKey(password, code, keyBits, blockSize, out byte[] key, out byte[] vektor);

        //    ICryptoTransform cryptoTransform;
        //    if (encrypt)
        //    {
        //        cryptoTransform = aes_provider.CreateEncryptor(key, vektor);
        //    }
        //    else
        //    {
        //        cryptoTransform = aes_provider.CreateDecryptor(key, vektor);
        //    }

        //    //try
            
        //    //{
        //        using (CryptoStream cryptoStream = new CryptoStream(output, cryptoTransform, CryptoStreamMode.Write))
        //        {
        //            const int memorySize = 1;
        //            byte[] buffor = new byte[memorySize];
        //            int readBits;
        //            while (true)
        //            {
        //                readBits = input.Read(buffor, 0, memorySize);
        //                //if (readBits == 0) break;

        //                cryptoStream.Write(buffor, 0, readBits);
        //           cryptoStream.Close();
        //            }
        //        }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show(ex.Message + ex.Source + ex.StackTrace);
        //    //}
        //    cryptoTransform.Dispose();
        //}

        //private static void GenerateEncryprionKey(string password, byte[] code, int keyBits, int blockSize, out byte[] key, out byte[] vektor)
        //{
        //    Rfc2898DeriveBytes mixedBits = new Rfc2898DeriveBytes(password, code, 1500);

        //    key = mixedBits.GetBytes(keyBits / 8);
        //    vektor = mixedBits.GetBytes(blockSize / 8);
        //}
    }
}
