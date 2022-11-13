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

        #region Index
        public IEnumerable<SelectListItem> JobRecordsCaseStatusItemList { get; set; }  //用來存放JobRecordsCaseStatusItem資料表中的Item

        public IEnumerable<SelectListItem> JobRecordsOnsiteItemList { get; set; }   //用來存放JobRecordsOnsiteList資料表中的Item

        public IEnumerable<SelectListItem> JobRecordsLocationItemList { get; set; }   //用來存放JobRecordsLocationItem資料表中的Item

        public IEnumerable<SelectListItem> JobRecordsProductTypeList { get; set; }   //用來存放JobRecordsProductType資料表中的Item

        public IEnumerable<SelectListItem> JobRecordsOSVersionList { get; set; }   //用來存放JobRecordsOSVersion資料表中的Item

        public IEnumerable<SelectListItem> JobRecordsCategoryList { get; set; }   //用來存放JobRecordsCategory資料表中的Item
        #endregion

        #region CreateCase
        public List<JobRecordsLocationItem> LocationList { get; set; }
        public List<JobRecordsProductType> ProductTypeList { get; set; }
        public List<JobRecordsOSVersion> OSVersionList { get; set; }
        public List<JobRecordsCategory> CategoryList { get; set; }

        //public string[] CaseCategory { get; set; }
        //public string OnsiteName { get; set; }
        #endregion
    }
}
