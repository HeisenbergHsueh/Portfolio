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
    public class MailService
    {
        public readonly IWebHostEnvironment _env;

        public MailService(IWebHostEnvironment env)
        {
            _env = env;
        }

        //gmail使用程式寄信所需做的相關設定 : https://www.webdesigntooler.com/google-smtp-send-mail
        //google應用程式驗證碼 : pxfxlongnhdcqngs
        private string mail_account = "heisenberghsueh";
        private string mail_password = "pxfxlongnhdcqngs";
        private string mail_address = "heisenberghsueh@gmail.com";

        /// <summary>
        /// 寄信的方法
        /// </summary>
        public void SendMail(string RecipientMailAddress)
        {
            //讀取信件範本
            string MailSample = File.ReadAllText(_env.WebRootPath.ToString() + @"\Mail\RegisterEmailTemplate.html");
            
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
            //設定信件內容
            mail.Body = MailSample;
            //設定信件格式為HTML
            mail.IsBodyHtml = true;

            //攜帶附件
            Attachment attachment = new Attachment(_env.WebRootPath.ToString() + @"\Mail\attachment.txt");
            //加入附件至mail中
            mail.Attachments.Add(attachment);

            //如何內嵌圖片至信件內容中 : https://blog.miniasp.com/post/2008/02/06/How-to-send-Email-with-embedded-picture-image
            string ImagePath = _env.WebRootPath.ToString() + @"\Images\Capoo_Hi.jpg";
            Attachment Image = new Attachment(ImagePath);
            Image.Name = Path.GetFileName(ImagePath);
            Image.NameEncoding = Encoding.GetEncoding("utf-8");
            Image.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

            //設定該附件為一個內嵌附件(inline attachment)
            Image.ContentDisposition.Inline = true;
            Image.ContentDisposition.DispositionType = System.Net.Mime.DispositionTypeNames.Inline;

            mail.Attachments.Add(Image);

            //送出信件
            SMTPClient.Send(mail);
        }
    }
}
