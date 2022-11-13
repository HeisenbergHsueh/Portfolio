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
//cookie授權
using Microsoft.AspNetCore.Authorization;

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
        public IActionResult JobRecordSystemIndex(int? CaseId, int? CaseStatus, int? Location, int? ProductType, int? OSVersion, string Category, string OnsiteName, string UserName, string HostName, int Page = 1, int PageSize = 5)
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

            if (ProductType != null)
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.ProductType == ProductType);
            }

            if (OSVersion != null)
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.OSVersion == OSVersion);
            }

            if (!string.IsNullOrEmpty(Category))
            {
                GetJobRecordsAllData = GetJobRecordsAllData.Where(j => j.Category.Contains(Category));
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
            ViewData["ProductType"] = ProductType;
            ViewData["OSVersion"] = OSVersion;
            ViewData["Category"] = Category;
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

            #region Product Item
            var GetJobRecordsProductTypeItem = _db.JobRecordsProductType;

            List<SelectListItem> JobRecordsProductTypeItemList = new List<SelectListItem>();

            foreach(var item in GetJobRecordsProductTypeItem)
            {
                JobRecordsProductTypeItemList.Add(new SelectListItem() 
                {
                    Text = item.ProductName,
                    Value = item.ProductId.ToString()
                });
            }
            #endregion

            #region OSVersion Item
            var GetJobRecordsOSVersionItem = _db.JobRecordsOSVersion;

            List<SelectListItem> JobRecordsOSVersionItemList = new List<SelectListItem>();

            foreach (var item in GetJobRecordsOSVersionItem)
            {
                JobRecordsOSVersionItemList.Add(new SelectListItem()
                {
                    Text = item.OSVersionName,
                    Value = item.OSVersionId.ToString()
                });
            }
            #endregion

            #region Category Item
            var GetJobRecordsCategoryItem = _db.JobRecordsCategory;

            List<SelectListItem> JobRecordsCategoryItemList = new List<SelectListItem>();

            foreach (var item in GetJobRecordsCategoryItem)
            {
                JobRecordsCategoryItemList.Add(new SelectListItem()
                {
                    Text = item.CategoryName,
                    Value = item.CategoryId.ToString()
                });
            }
            #endregion

            model.JobRecordsOnsiteItemList = JobRecordsOnsiteItemList;
            model.JobRecordsCaseStatusItemList = JobRecordsCaseStatusItemList;
            model.JobRecordsLocationItemList = JobRecordsLocationItemList;
            model.JobRecordsProductTypeList = JobRecordsProductTypeItemList;
            model.JobRecordsOSVersionList = JobRecordsOSVersionItemList;
            model.JobRecordsCategoryList = JobRecordsCategoryItemList;

            #endregion

            //將所query出來的資料，依照ID由大到小排序，轉成ToList之後，再轉成PagedList，然後再放入ViewModel中的PagedList中
            model.JobRecordsPagedList = GetJobRecordsAllData.OrderByDescending(j => j.CaseId).ToList().ToPagedList(Page, PageSize);
          
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

            //將與此電腦名稱相同的過往歷史案件撈出來
            var GetHistoryCaseRecords = await _db.JobRecords.Where(j => j.HostName == GetJobRecordsSingleData.HostName).Where(j => j.BuildDate.CompareTo(GetJobRecordsSingleData.BuildDate)< 0 ).ToListAsync();

            JobRecordsSingleCaseViewModel model = new JobRecordsSingleCaseViewModel();

            #region 將Item由數字轉成對應的分類文字
            model.CaseStatusList = await (from j in _db.JobRecordsCaseStatusItem select j).ToListAsync();
            model.LocationList = await (from j in _db.JobRecordsLocationItem select j).ToListAsync();
            model.ProductTypeList = await (from j in _db.JobRecordsProductType select j).ToListAsync();
            model.OSVersionList = await (from j in _db.JobRecordsOSVersion select j).ToListAsync();
            model.CategoryList = await (from j in _db.JobRecordsCategory select j).ToListAsync();
            #endregion

            #region 將Category Item由數字轉成對應的分類文字
            //Load Category Item
            List<JobRecordsCategory> CategoryList = await (from c in _db.JobRecordsCategory select c).ToListAsync();

            //將單筆案件內容中的Category Item Id以逗號做分開，並轉成string array
            string[] CategorySplitToStringArr = GetJobRecordsSingleData.Category.Split(",");

            //宣告一個新的List，接著會進行Item的比對，並將比對到的Category Name存放在此List
            List<string> CategoryJoinStringArr = new List<string>();

            //開始比對
            foreach (var item in CategorySplitToStringArr)
            {
                for (var i = 0; i < (CategoryList.Count() - 1); i++)
                {
                    if(item == CategoryList[i].CategoryId.ToString())
                    {
                        CategoryJoinStringArr.Add(CategoryList[i].CategoryName);
                    }
                } 
            }

            //將List中的Category Name以"、"做Join轉成string
            string CategoryResult = String.Join("、", CategoryJoinStringArr);

            //最後再將轉完的結果放回單一案件的Category欄位中
            //如此呈現的結果為 : 原本資料庫中的Category欄位值為"1,3" => 轉完後變成"產品問題、網路問題" 
            GetJobRecordsSingleData.Category = CategoryResult;

            #endregion

            model.JobRecordsModel = GetJobRecordsSingleData;
            model.JobRecordsReplyModel = GetJobRecordsReplyRelatedData;
            model.JobRecordsEnum = GetHistoryCaseRecords;

            return View(model);
        }
        #endregion

        #region 駐場人員建案系統-新建案件(Create)
        [HttpGet]
        public async Task<IActionResult> JobRecordsCreateCase(JobRecordsViewModel model)
        {
            model.LocationList = await (from j in _db.JobRecordsLocationItem select j).ToListAsync();
            model.ProductTypeList = await (from j in _db.JobRecordsProductType select j).ToListAsync();
            model.OSVersionList = await (from j in _db.JobRecordsOSVersion select j).ToListAsync();
            model.CategoryList = await (from j in _db.JobRecordsCategory select j).ToListAsync();

            return View(model);
        }

        [HttpPost, ActionName(nameof(JobRecordsCreateCase))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JobRecordsCreateCaseComfirm(JobRecordsViewModel model, string[] CaseCategory)
        {
            int GetLastCaseIdFromDB = _db.JobRecords.Max(c => c.CaseId);

            model.JobRecordsModel.CaseId = GetLastCaseIdFromDB + 1;

            model.JobRecordsModel.BuildDate = DateTime.Now;

            model.JobRecordsModel.CaseStatus = 1;

            model.JobRecordsModel.Category = String.Join(",", CaseCategory);

            model.JobRecordsModel.ClosedDate = null;

            model.JobRecordsModel.ClosedOnsiteName = null;

            if(ModelState.IsValid)
            {
                _db.Add(model.JobRecordsModel);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(JobRecordSystemIndex), "JobRecordSystem");
            }
            
            return View(model.JobRecordsModel);
        }
        #endregion

        #region 駐場人員建案系統-禁止瀏覽頁面(Forbidden)
        #endregion

    }
}
