using SP_SanHtar.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.cls
{
    public class Hash
    {
        public static async Task<string> Encrypt(CryptographyClass tempData)
        {
            try
            {
                tempData.PlainText = await Task.Run((() =>
                {
                    return Encoding.UTF8.GetBytes(tempData.Text);

                }));

                return await SHA512_Encrypt(tempData);
            }
            catch
            {
                return string.Empty;
            }
        }

        private static async Task<string> SHA512_Encrypt(CryptographyClass tempData)
        {
            try
            {
                byte[] plainTextWithSaltBytes = new byte[tempData.PlainText.Length + tempData.SALT.Length];

                for (int i = 0; i < tempData.PlainText.Length; i++)
                {
                    plainTextWithSaltBytes[i] = tempData.PlainText[i];
                }

                for (int i = 0; i < tempData.SALT.Length; i++)
                {
                    plainTextWithSaltBytes[tempData.PlainText.Length + i] = tempData.SALT[i];
                }

                byte[] hashBytes = await Task.Run((() =>
                {
                    return new SHA512Managed().ComputeHash(plainTextWithSaltBytes);
                }));

                byte[] hashWithSaltBytes = new byte[hashBytes.Length + tempData.SALT.Length];

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashWithSaltBytes[i] = hashBytes[i];
                }

                for (int i = 0; i < tempData.SALT.Length; i++)
                {
                    hashWithSaltBytes[hashBytes.Length + i] = tempData.SALT[i];
                }

                return await Task.Run((() =>
                {
                    return Convert.ToBase64String(hashWithSaltBytes);
                }));
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task<bool> CompareHashValue(CryptographyClass tempData, string OldHASHValue)
        {
            try
            {
                var enpassword= Encrypt(tempData);
                return (await Encrypt(tempData)).Equals(OldHASHValue);
            }
            catch
            {
                return false;
            }
        }
    }

    public class AES
    {
        private async static Task<byte[]> AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            byte[] saltBytes = passwordBytes;
            await Task.Run((() =>
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;
                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                        AES.Mode = CipherMode.CBC;
                        using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        encryptedBytes = ms.ToArray();
                    }
                }
            }));
            return encryptedBytes;
        }

        private async static Task<byte[]> AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;
            byte[] saltBytes = passwordBytes;
            await Task.Run((() =>
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;
                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                        AES.Mode = CipherMode.CBC;
                        using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }
            }));
            return decryptedBytes;
        }

        public static async Task<string> Encrypt(CryptographyClass tempData)
        {
            byte[] originalBytes = Encoding.UTF8.GetBytes(tempData.Text);
            byte[] encryptedBytes = null;
            tempData.SALT = await Task.Run((() =>
            {
                return SHA256.Create().ComputeHash(tempData.SALT);
            }));
            int saltSize = await GetSaltSize(tempData.SALT);
            byte[] saltBytes = await GetRandomBytes(saltSize);
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < originalBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            }
            encryptedBytes = await AES_Encrypt(bytesToBeEncrypted, tempData.SALT);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static async Task<string> Decrypt(CryptographyClass tempData)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(tempData.Text);
            tempData.SALT = await Task.Run((() =>
            {
                return SHA256.Create().ComputeHash(tempData.SALT);
            }));
            byte[] decryptedBytes = await AES_Decrypt(bytesToBeDecrypted, tempData.SALT);
            int saltSize = await GetSaltSize(tempData.SALT);
            byte[] originalBytes = new byte[decryptedBytes.Length - saltSize];
            for (int i = saltSize; i < decryptedBytes.Length; i++)
            {
                originalBytes[i - saltSize] = decryptedBytes[i];
            }
            return Encoding.UTF8.GetString(originalBytes);
        }

        private async static Task<int> GetSaltSize(byte[] passwordBytes)
        {
            var key = await Task.Run((() =>
            {
                return new Rfc2898DeriveBytes(passwordBytes, passwordBytes, 1000);
            }));
            byte[] ba = key.GetBytes(2);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ba.Length; i++)
            {
                sb.Append(Convert.ToInt32(ba[i]).ToString());
            }
            int saltSize = 0;
            string s = sb.ToString();
            foreach (char c in s)
            {
                int intc = Convert.ToInt32(c.ToString());
                saltSize = saltSize + intc;
            }

            return saltSize;
        }

        private async static Task<byte[]> GetRandomBytes(int length)
        {
            byte[] ba = new byte[length];
            await Task.Run((() =>
            {
                RNGCryptoServiceProvider.Create().GetBytes(ba);
            }));
            return ba;
        }
    }

    public class GeneratePassword
    {
        private Random random = new Random();

        public string RandomString(int length)
        {
            const string chars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
