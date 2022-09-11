using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

//正規表達式
using System.Text.RegularExpressions;

namespace Portfolio.Security
{
    public class CipherServices
    {
        /// <summary>
        /// 將明文加密
        /// </summary>
        /// <param name="PlainText">明文</param>
        /// <param name="PublicKey">由appsettings.json讀取到的公鑰字串</param>
        /// <returns></returns>
        public string PlainTextToCipher(string PlainText, string PublicKey)
        {
            string result = AES_EncryptToBase64(PlainText, PublicKey);

            return result;
        }

        /// <summary>
        /// 將密文解密
        /// </summary>
        /// <returns></returns>
        public string CipherToPlainText(string Cipher, string PublicKey)
        {
            string result = AES_DecryptToString(Cipher, PublicKey);

            return result;
        }

        /// <summary>
        /// AES雙向加密
        /// 參考書籍 : 380個精選實例：一步步昇華成.NET Core大內高手
        /// </summary>
        /// <returns></returns>
        private byte[] AES_BidirectionalEncryptData(byte[] key, byte[] iv, string content)
        {

            byte[] res = null;

            using (Aes aes = Aes.Create())
            {
                using (MemoryStream msbase = new MemoryStream())
                {
                    using (CryptoStream cstr = new CryptoStream(msbase, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cstr))
                        {
                            writer.Write(content);
                        }
                    }
                    res = msbase.ToArray();
                }
            }

            return res;
        }

        /// <summary>
        /// AES單向加密
        /// 參考書籍 : 380個精選實例：一步步昇華成.NET Core大內高手
        /// </summary>
        /// <returns></returns>
        private string AES_SingleDirectionalEncryptData()
        {
            string result = "";

            return result;
        }

        /// <summary>
        /// 字串加密(AES非對稱式加密)
        /// </summary>
        /// <param name="PlainText">明文</param>
        /// <param name="PublicKey">公鑰</param>
        /// <returns></returns>
        private string AES_EncryptToBase64(string PlainText, string PublicKey)
        {
            string result = "";

            //將傳進來的PublicKey再進行一次字串的處理
            //本作品集會將從appsettings.json讀取到的公鑰處理完之後變成私鑰使用
            //而字串處理的方式有很多種，那本作品集使用的是將字串後六位數字去除，剩下的字串當私鑰使用
            //因此就是將 HeisenbergHsueh123456 改成 HeisenbergHsueh
            string PrivateKey = Regex.Replace(PublicKey, "[0-9]", "", RegexOptions.IgnoreCase);

            try
            {
                AesCryptoServiceProvider AES = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
                SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider();

                byte[] key = SHA256.ComputeHash(Encoding.UTF8.GetBytes(PrivateKey));
                byte[] iv = MD5.ComputeHash(Encoding.UTF8.GetBytes(PrivateKey));
                AES.Key = key;
                AES.IV = iv;

                byte[] DataToByteArray = Encoding.UTF8.GetBytes(PlainText);

                using (MemoryStream MS = new MemoryStream())
                {
                    using (CryptoStream CS = new CryptoStream(MS, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        CS.Write(DataToByteArray, 0, DataToByteArray.Length);
                        CS.FlushFinalBlock();
                        result = Convert.ToBase64String(MS.ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        /// <summary>
        /// 密文解密(AES非對稱式解密)
        /// </summary>
        /// <param name="Cipher">密文</param>
        /// <param name="PrivateKey">密鑰</param>
        /// <returns></returns>
        private string AES_DecryptToString(string Cipher, string PublicKey)
        {
            string result = "";

            //將傳進來的PublicKey再進行一次字串的處理
            //本作品集會將從appsettings.json讀取到的公鑰處理完之後變成私鑰使用
            //而字串處理的方式有很多種，那本作品集使用的是將字串後六位數字去除，剩下的字串當私鑰使用
            //因此就是將 HeisenbergHsueh123456 改成 HeisenbergHsueh
            string PrivateKey = Regex.Replace(PublicKey, "[0-9]", "", RegexOptions.IgnoreCase);

            try
            {
                AesCryptoServiceProvider AES = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
                SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider();

                byte[] key = SHA256.ComputeHash(Encoding.UTF8.GetBytes(PrivateKey));
                byte[] iv = MD5.ComputeHash(Encoding.UTF8.GetBytes(PrivateKey));
                AES.Key = key;
                AES.IV = iv;

                byte[] DataToByteArray = Convert.FromBase64String(Cipher);

                using (MemoryStream MS = new MemoryStream())
                {
                    using (CryptoStream CS = new CryptoStream(MS, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        CS.Write(DataToByteArray, 0, DataToByteArray.Length);
                        CS.FlushFinalBlock();
                        result = Encoding.UTF8.GetString(MS.ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }
    }
}
