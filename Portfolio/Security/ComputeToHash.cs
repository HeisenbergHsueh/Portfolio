using System;
using System.Text;
using System.Security.Cryptography;

//Ref : https://www.twblogs.net/a/5d43a291bd9eee5327fb2f15

namespace Portfolio.Security
{
    public class ComputeToHash
    {
        internal static string StringToHash(string PlainText, string ChooseAlgo)
        {
            string result = String.Empty;

            if (ChooseAlgo == "MD5")
            {
                result = ComputeToMD5(PlainText);
            }
            else if (ChooseAlgo == "SHA1")
            {
                result = ComputeToSHA1(PlainText);
            }
          
            return result;
        }

        /// <summary>
        /// 將傳進來的字串，加密成MD5的hash
        /// </summary>
        /// <param name="PlainText">明文</param>
        /// <returns></returns>
        private static string ComputeToMD5(string PlainText)
        {
            using (var cryptoMD5 = MD5.Create())
            {
                //將plaintext先轉成UTF8的位元陣列
                var bytes = Encoding.UTF8.GetBytes(PlainText);

                //將位元陣列換算成雜湊
                var hash = cryptoMD5.ComputeHash(bytes);

                //取得
                var hashcode = BitConverter.ToString(hash).Replace("-", String.Empty).ToUpper();

                return hashcode;
            }          
        }

        /// <summary>
        /// 將傳進來的字串，加密成SHA1的hash
        /// </summary>
        /// <param name="PlainText">明文</param>
        /// <returns></returns>
        private static string ComputeToSHA1(string PlainText)
        {
            using (var cryptoSHA1 = SHA1.Create())
            {
                //將plaintext先轉成UTF8的位元陣列
                var bytes = Encoding.UTF8.GetBytes(PlainText);

                //將位元陣列換算成雜湊
                var hash = cryptoSHA1.ComputeHash(bytes);

                //取得
                var hashcode = BitConverter.ToString(hash).Replace("-", String.Empty).ToUpper();

                return hashcode;
            }
        }
    }
}
