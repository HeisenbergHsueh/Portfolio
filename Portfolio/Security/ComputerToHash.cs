using System;
using System.Text;
using System.Security.Cryptography;

namespace Portfolio.Security
{
    public class ComputerToHash
    {
        /// <summary>
        /// 將傳進來的字串，加密成MD5的hash
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <returns></returns>
        public static string ComputerToMD5(string plaintext)
        {
            using (var cryptoMD5 = MD5.Create())
            {
                //將plaintext先轉成UTF8的位元陣列
                var bytes = Encoding.UTF8.GetBytes(plaintext);

                //將位元陣列換算成雜湊
                var hash = cryptoMD5.ComputeHash(bytes);

                //取得
                var hashcode = BitConverter.ToString(hash).Replace("-", String.Empty).ToUpper();

                return hashcode;
            }          
        }
    }
}
