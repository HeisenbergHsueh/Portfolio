using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//================================================= JobsRecordViewModel 所會使用到的命名空間 =================================================
using Portfolio.Models;
using Portfolio.Models.JobRecordSystem;
//================================================= 分頁所會使用到的命名空間 =================================================
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portfolio.Models.JobRecordSystem
{
    public class JobRecordsViewModel
    {
        public JobRecords JobRecordsVM { get; set; }

        public IPagedList<JobRecords> JobRecordsPagedList { get; set; }

        public JobRecordsStatus jobrecordsStatus { get; set; }

        public List<JobsRecordStatusListItem> StatusCBVM { get; set; }  //StatusCBVM = StatusCheckBoxViewModel

        public IEnumerable<SelectListItem> JobRecordsStatusItemList { get; set; }  //用來存放 JobsRecordStatus 資料表中的 Item

        public IEnumerable<SelectListItem> JobRecordsSiteItemList { get; set; }  //用來存放 JobsRecordSite 資料表中的 Item

        public IEnumerable<SelectListItem> JobRecordsOwnerItemList { get; set; }  //用來存放 JobsRecordOwner 資料表中的 Item

        public IEnumerable<SelectListItem> OSVersionSelectListItem { get; set; }  //用來存放 JobsRecordOSVersion 資料表中的 Item

        public IEnumerable<SelectListItem> ProductTypeSelectListItem { get; set; }  //用來存放 JobsRecordProductType 資料表中的 Item

        public IEnumerable<SelectListItem> CategorySelectListItem { get; set; }  //用來存放 JobsRecordProductType 資料表中的 Item
    }
  
    public class JobsRecordStatusListItem
    {
        public int SId { get; set; }

        public string SName { get; set; }

        public bool IsChecked { get; set; }
    }

    // 使用 ViewModel（檔名 JobsRecordViewModel.cs），新增一筆記錄時，用到 CheckBoxList
    // 資料來源：https://forums.asp.net/t/2071058.aspx?how+to+create+CheckBoxList+In+MVC+Razor+5
    // 資料來源：https://www.c-sharpcorner.com/UploadFile/0c1bb2/how-to-bind-checkboxlist-in-Asp-Net-mvc/   （兩者作法雷同）
}
