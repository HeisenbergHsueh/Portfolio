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
        public JobRecords JobRecordsModel { get; set; }

        public IPagedList<JobRecords> JobRecordsPagedList { get; set; }

        public IEnumerable<SelectListItem> JobRecordsCaseStatusItemList { get; set; }  //用來存放JobRecordsCaseStatusItem資料表中的Item

        public IEnumerable<SelectListItem> JobRecordsOnsiteItemList { get; set; }   //用來存放JobRecordsOnsiteList資料表中的Item

        public IEnumerable<SelectListItem> JobRecordsLocationItemList { get; set; }   //用來存放JobRecordsLocationItem資料表中的Item

        public List<JobsRecordStatusListItem> StatusCBVM { get; set; }  //StatusCBVM = StatusCheckBoxViewModel

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
