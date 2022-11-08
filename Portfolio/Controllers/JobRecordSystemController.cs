using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Data;
using Portfolio.Models.JobRecordSystem;
using X.PagedList;
using X.PagedList.Mvc.Core;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult JobRecordSystemIndex(int? CaseId, int? CaseStatus, int? Location, string OnsiteName, string UserName, string HostName, int Page = 1, int PageSize = 5)
        {
            //宣告一個新的JobRecordsViewModel
            JobRecordsViewModel model = new JobRecordsViewModel();

            var GetJobRecordsAllData = from j in _db.JobRecords select j;

            #region LINQ多條件搜尋
            if (CaseId != null)
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.CaseId == CaseId);
            }

            if (CaseStatus != null)
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.CaseStatus == CaseStatus);
            }

            if (Location != null)
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.Location == Location);
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

            #region 使用ViewData存放user輸入的關鍵字
            ViewData["CaseId"] = CaseId;
            ViewData["CaseStatus"] = CaseStatus;
            ViewData["Location"] = Location;
            ViewData["OnsiteName"] = OnsiteName;
            ViewData["UserName"] = UserName;
            ViewData["HostName"] = HostName;
            ViewData["PageSize"] = PageSize;
            #endregion

            #region 載入表單中，需要使用dropdownlist、checkbox、radiobox呈現的欄位

            #region Onsite Item
            //從DB中的JobRecordsOnsiteList中，獲得所有Item的資料
            var GetJobRecordsOnsiteListItem = _db.JobRecordsOnsiteList;

            //宣告一個 List<SelectListItem>的物件
            List<SelectListItem> JobRecordsOnsiteItemList = new List<SelectListItem>();

            //把獲得的Onsite Item全部裝入此List中
            foreach (var item in GetJobRecordsOnsiteListItem)
            {
                JobRecordsOnsiteItemList.Add(new SelectListItem()
                {
                    Text = item.OnsiteName,
                    Value = item.OnsiteName,
                });
            }
            #endregion

            #region CaseStatus Item
            var GetJobRecordsCaseStatusItem = _db.JobRecordsCaseStatusItem;

            List<SelectListItem> JobRecordsCaseStatusItemList = new List<SelectListItem>();

            foreach (var item in GetJobRecordsCaseStatusItem)
            {
                JobRecordsCaseStatusItemList.Add(new SelectListItem()
                {
                    Text = item.CaseStatusName,
                    Value = item.CaseStatusId.ToString(),
                });
            }
            #endregion

            #region Location Item
            var GetJobRecordsLocationItem = _db.JobRecordsLocationItem;

            List<SelectListItem> JobRecordsLocationItemList = new List<SelectListItem>();

            foreach (var item in GetJobRecordsLocationItem)
            {
                JobRecordsLocationItemList.Add(new SelectListItem()
                {
                    Text = item.LocationName,
                    Value = item.LocationId.ToString()
                });
            }
            #endregion

            model.JobRecordsOnsiteItemList = JobRecordsOnsiteItemList;
            model.JobRecordsCaseStatusItemList = JobRecordsCaseStatusItemList;
            model.JobRecordsLocationItemList = JobRecordsLocationItemList;            

            #endregion

            //宣告一個新的JobRecordsViewModel
            //將所query出來的資料，轉成ToList之後，再轉成PagedList，然後再放入ViewModel中的PagedList中
            model.JobRecordsPagedList = GetJobRecordsAllData.ToList().ToPagedList(Page, PageSize);
          
            return View(model);
        }
        #endregion

        #region 駐場人員建案系統-單一Case的詳細內容(Detail)
        public async Task<IActionResult> JobRecordsSingleCaseDetail(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            //撈取案件編號與輸入id相同的案件資料
            var GetJobRecordsSingleData = await _db.JobRecords.FirstOrDefaultAsync(j => j.CaseId == id);

            if(GetJobRecordsSingleData == null)
            {
                return NotFound();
            }

            //撈取與該案件編號相關聯的留言內容
            var GetJobRecordsReplyRelatedData = await _db.JobRecordsReply.Where(j => j.RelatedWithJobRecordsId == id).OrderByDescending(j => j.ReplyDateTime).ToListAsync();

            var GetHistoryCaseRecords = await _db.JobRecords.Where(j => j.HostName == GetJobRecordsSingleData.HostName).Where(j => j.BuildDate.CompareTo(GetJobRecordsSingleData.BuildDate)< 0 ).ToListAsync();

            JobRecordsSingleCaseViewModel model = new JobRecordsSingleCaseViewModel();

            model.JobRecordsModel = GetJobRecordsSingleData;

            model.JobRecordsReplyModel = GetJobRecordsReplyRelatedData;

            model.JobRecordsEnum = GetHistoryCaseRecords;

            return View(model);
        }
        #endregion

        #region 駐場人員建案系統-新建案件(Create)
        public async Task<IActionResult> JobRecordsCreateCase(JobRecordsViewModel model)
        {
            var GetJobRecordsCaseStatusItem = await (from j in _db.JobRecordsCaseStatusItem select j).ToListAsync();

            List<SelectListItem> JobRecordsCaseStatusItemList = new List<SelectListItem>();

            foreach (var item in GetJobRecordsCaseStatusItem)
            {
                JobRecordsCaseStatusItemList.Add(new SelectListItem()
                {
                    Text = item.CaseStatusName,
                    Value = item.CaseStatusId.ToString()
                });
            }

            model.JobRecordsCaseStatusItemList = JobRecordsCaseStatusItemList;

            return View();
        }

        [HttpPost, ActionName(nameof(JobRecordsCreateCase))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JobRecordsCreateCaseComfirm(JobRecordsViewModel model)
        {
            return View();
        }
        #endregion

    }
}
