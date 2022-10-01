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
        [DisplayName("案件編號")]
        public int Id { get; set; }

        [DisplayName("案件狀態")]
        [Required(ErrorMessage = "此欄位不可以空白")]
        public string Status { get; set; }

        [DisplayName("案件建立日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayName("案件結束日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [DisplayName("案件更新日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdateTime { get; set; }

        [DisplayName("案件問題描述")]
        [Required(ErrorMessage = "此欄位不可以空白")]
        public string Description { get; set; }

        [DisplayName("案件處理過程描述")]
        [Required(ErrorMessage = "此欄位不可以空白")]
        public string Progress { get; set; }

        [DisplayName("案件所在廠區")]
        public string Site { get; set; }

        [DisplayName("報案人員")]
        [Required(ErrorMessage = "此欄位不可以空白")]
        [StringLength(50, ErrorMessage = "字串長度不可以超過50個字元")]
        public string UserName { get; set; }

        [DisplayName("處理人員")]
        [Required(ErrorMessage = "此欄位不可以空白")]
        [StringLength(50, ErrorMessage = "字串長度不可以超過50個字元")]
        public string OwnerName { get; set; }

        [DisplayName("報案電腦名稱")]
        [Required(ErrorMessage = "此欄位不可以空白")]
        [StringLength(50, ErrorMessage = "字串長度不可以超過50個字元")]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9]{7}$", ErrorMessage = "只允數輸入8位的英文(大小寫皆可)數字")]  //正規表達式 : 只允數輸入8位的英文(大小寫皆可)數字
        public string Hostname { get; set; }

        [DisplayName("報案產品類型")]
        public string ProductType { get; set; }

        [DisplayName("報案電腦系統類型")]
        public string Osversion { get; set; }

        [DisplayName("案件問題分類")]
        public string Category { get; set; }
    }
}
