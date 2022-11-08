using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//=================================================== 資料欄位驗證會使用到的命名空間 ===================================================
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Compression;

namespace Portfolio.Models.JobRecordSystem
{
    public class JobRecordsReply
    {
        [Key]
        public int ReplyId { get; set; }

        public int RelatedWithJobRecordsId { get; set; }

        public string ReplyPersonName { get; set; }

        public DateTime ReplyDateTime { get; set; }

        public string ReplyContent { get; set; }

        public string IsThereAttachment { get; set; }

        public string AttachmentName { get; set; }
    }
}
