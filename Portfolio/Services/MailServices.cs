using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

//讀取wwwroot靜態檔案的位置
using Microsoft.AspNetCore.Hosting;
//寄信用
using System.Net.Mail;

namespace Portfolio.Services
{
    /// <summary>
    /// 此類別用來撰寫各式各樣與mail有關的服務
    /// 例如 : 會員註冊認證信、重設密碼認證信等等
    /// </summary>
    public class MailServices
    {
        //gmail使用程式寄信所需做的相關設定 : https://www.webdesigntooler.com/google-smtp-send-mail
        //google應用程式驗證碼 : pxfxlongnhdcqngs
        private string mail_account = "heisenberghsueh";
        private string mail_password = "pxfxlongnhdcqngs";
        private string mail_address = "heisenberghsueh@gmail.com";

        #region 產生驗證碼
        /// <summary>
        /// 隨機產生50個大小寫英文加數字組合的驗證碼
        /// </summary>
        /// <returns></returns>
        public string GenerateAuthCode()
        {
            //宣告要回傳的驗證碼初始值
            string ValidateCode = string.Empty;
            
            //設定含有英文字母大小寫+數字的 string array
            string[] LetterArray = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            
            //宣告可產生隨機值的object
            Random random = new Random();

            //使用for迴圈產生50個隨機字母數字構成的字串當作驗證碼
            for (int i = 0; i < 50; i++)
            {
                ValidateCode += LetterArray[random.Next(LetterArray.Count())];
            }

            return ValidateCode;
        }
        #endregion

        #region 寄信
        /// <summary>
        /// 寄信的方法
        /// </summary>
        public void SendMail(string RecipientMailAddress, string wwwrootPath, string MailBody)
        {            
            //建立寄信用SMTP協定，這裡使用Gmail為例
            SmtpClient SMTPClient = new SmtpClient("smtp.gmail.com");
            //設定Gmail使用的Port
            SMTPClient.Port = 587;
            //設定寄件人的帳號密碼
            SMTPClient.Credentials = new System.Net.NetworkCredential(mail_account, mail_password);
            //開啟SSL加密傳輸
            SMTPClient.EnableSsl = true;


            //宣告信件的物件，接著會利用此物件設定信件的寄件人、收件人、信件內容、格式等
            MailMessage mail = new MailMessage();
            //設定寄件人信箱
            mail.From = new MailAddress(mail_address);
            //設定收件人信箱
            mail.To.Add(RecipientMailAddress);
            //設定信件主旨
            mail.Subject = "HeisenbergHsueh Portfolio 會員註冊認證信";
            

            //攜帶附件(包括要內嵌於信件內容中使用的圖片)
            //string attachment_file = _env.WebRootPath.ToString() + @"\Mail\attachment.txt";
            string attachment_image = wwwrootPath + @"\Images\Capoo_Hi.jpg";
            
            Attachment attachment = new Attachment(attachment_image);

            //如何內嵌圖片至信件內容中 : https://blog.miniasp.com/post/2008/02/06/How-to-send-Email-with-embedded-picture-image

            attachment.Name = Path.GetFileName(attachment_image);
            attachment.NameEncoding = Encoding.GetEncoding("utf-8");
            attachment.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

            //設定該附件為一個內嵌附件(inline attachment)
            attachment.ContentDisposition.Inline = true;
            attachment.ContentDisposition.DispositionType = System.Net.Mime.DispositionTypeNames.Inline;

            //加入附件至mail中
            mail.Attachments.Add(attachment);

            MailBody = MailBody.Replace("{{Image}}", "cid:" + attachment.ContentId);

            //設定信件內容
            mail.Body = MailBody;
            //設定信件格式為HTML
            mail.IsBodyHtml = true;

            //送出信件
            SMTPClient.Send(mail);
        }
        #endregion
    }
}
