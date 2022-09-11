using System;
using System.Text;
using System.Security.Cryptography;

//參考資料(1) : https://www.twblogs.net/a/5d43a291bd9eee5327fb2f15

namespace Portfolio.Security
{
    public class ComputeToHash
    {
        #region 字串加密轉成hash
        internal string StringToHash(string PlainText, string ChooseAlgo)
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
        #endregion

        #region 使用MD5的演算法將字串加密成hash
        /// <summary>
        /// 將傳進來的字串，加密成MD5的hash
        /// </summary>
        /// <param name="PlainText">明文</param>
        /// <returns></returns>
        private string ComputeToMD5(string PlainText)
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
        #endregion

        #region 使用SHA1的演算法將字串加密成hash
        /// <summary>
        /// 將傳進來的字串，加密成SHA1的hash
        /// </summary>
        /// <param name="PlainText">明文</param>
        /// <returns></returns>
        private string ComputeToSHA1(string PlainText)
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
        #endregion
    }
}
