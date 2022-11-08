using System;
using System.Collections.Generic;
//=================================================== 資料欄位驗證會使用到的命名空間 ===================================================
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Compression;

namespace Portfolio.Models.JobRecordSystem
{
    public partial class JobRecords
    {
        [Key]
        public int CaseId { get; set; }

        [Required(ErrorMessage = "此欄位不可以空白")]
        public int CaseStatus { get; set; }

        [Required(ErrorMessage = "此欄位不可以空白")]
        public string CaseTitle { get; set; }

        [DataType(DataType.DateTime)]
        //ApplyFormatInEditMode = true
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}")]
        public DateTime BuildDate { get; set; }

        [Required(ErrorMessage = "此欄位不可以空白")]
        public string CaseDescription { get; set; }

        public int Location { get; set; }

        [Required(ErrorMessage = "此欄位不可以空白")]
        [StringLength(50, ErrorMessage = "字串長度不可以超過50個字元")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "此欄位不可以空白")]
        [StringLength(50, ErrorMessage = "字串長度不可以超過50個字元")]
        public string OnsiteName { get; set; }

        [Required(ErrorMessage = "此欄位不可以空白")]
        [StringLength(50, ErrorMessage = "字串長度不可以超過50個字元")]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9]{7}$", ErrorMessage = "只允數輸入8位的英文(大小寫皆可)數字")]  //正規表達式 : 只允數輸入8位的英文(大小寫皆可)數字
        public string HostName { get; set; }

        public string ProductType { get; set; }

        public string OSVersion { get; set; }

        public int Category { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}")]
        public Nullable<DateTime> ClosedDate { get; set; }

        public string ClosedOnsiteName { get; set; }


    }
}
