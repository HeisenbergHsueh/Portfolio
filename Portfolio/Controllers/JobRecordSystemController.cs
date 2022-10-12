using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Data;
using Portfolio.Models.JobRecordSystem;
using X.PagedList;
using X.PagedList.Mvc.Core;

namespace Portfolio.Controllers
{
    public class JobRecordSystemController : Controller
    {
        #region 建構子
        private PortfolioContext _db;

        public JobRecordSystemController(PortfolioContext db)
        {
            _db = db;
        }
        #endregion

        #region 駐場人員建案系統-Index
        public IActionResult JobRecordSystemIndex(int? CaseId, string CaseStatus, string Location, string OnsiteName, string UserName, string HostName, int Page = 1, int PageSize = 5)
        {
            var GetJobRecordsAllData = from j in _db.JobRecords select j;

            #region LINQ多條件搜尋
            if (CaseId != null)
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.CaseId == CaseId);
            }

            if (!string.IsNullOrEmpty(CaseStatus))
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.CaseStatus.Contains(CaseStatus));
            }

            if (!string.IsNullOrEmpty(Location))
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.Location.Contains(Location));
            }

            if (!string.IsNullOrEmpty(OnsiteName))
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.OnsiteName.Contains(OnsiteName));
            }

            if (!string.IsNullOrEmpty(UserName))
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.UserName.Contains(UserName));
            }

            if (!string.IsNullOrEmpty(HostName))
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.HostName.Contains(HostName));
            }
            #endregion

            //使用ViewData存放user輸入的關鍵字
            ViewData["CaseId"] = CaseId;
            ViewData["CaseStatus"] = CaseStatus;
            ViewData["Location"] = Location;
            ViewData["OnsiteName"] = OnsiteName;
            ViewData["UserName"] = UserName;
            ViewData["HostName"] = HostName;
            ViewData["PageSize"] = PageSize;

            #region 載入表單各欄位的項目
            //從DB中的JobRecordsOwner的Table中，獲得Table中所有Item的資料
            var GetJobRecordsOwnerItem = _db.JobRecordsOwner;

            //宣告一個 List<SelectListItem>的物件
            List<SelectListItem> JobRecordsOwnerItemList = new List<SelectListItem>();

            //把獲得的Owner Item全部裝入此List中
            foreach (var item in GetJobRecordsOwnerItem)
            {
                JobRecordsOwnerItemList.Add(new SelectListItem()
                {
                    Text = item.OwnerName,
                    Value = item.OwnerName,
                });
            }

            var GetJobRecordsSiteItem = _db.JobRecordsSite;

            List<SelectListItem> JobRecordsSiteItemList = new List<SelectListItem>();

            foreach (var item in GetJobRecordsSiteItem)
            {
                JobRecordsSiteItemList.Add(new SelectListItem()
                {
                    Text = item.SiteName,
                    Value = item.SiteName,
                });
            }

            var GetJobRecordsStatusItem = _db.JobRecordsStatus;

            List<SelectListItem> JobRecordsStatusItemList = new List<SelectListItem>();

            foreach (var item in GetJobRecordsStatusItem)
            {
                JobRecordsStatusItemList.Add(new SelectListItem()
                {
                    Text = item.StatusName,
                    Value = item.StatusName,
                });
            }


            ViewBag.JobRecordsOwnerItemList = JobRecordsOwnerItemList;
            ViewBag.JobRecordsSiteItemList = JobRecordsSiteItemList;
            ViewBag.JobRecordsStatusItemList = JobRecordsStatusItemList;

            #endregion

            //宣告一個新的JobRecordsViewModel
            JobRecordsViewModel jobRecordsViewModel = new JobRecordsViewModel
            {
                //將所query出來的資料，轉成ToList之後，再轉成PagedList，然後再放入ViewModel中的PagedList中
                JobRecordsPagedList = GetJobRecordsAllData.ToList().ToPagedList(Page, PageSize)
            };
          
            return View(jobRecordsViewModel);
        }
        #endregion

    }
}
