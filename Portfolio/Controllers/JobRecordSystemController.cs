using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
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
    [Authorize]
    public class JobRecordSystemController : Controller
    {
        #region 建構子
        private PortfolioContext _db;

        private readonly string _folder;

        private readonly static Dictionary<string, string> _contentTypes = new Dictionary<string, string>
        {
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"}
        };

        public JobRecordSystemController(PortfolioContext db, IWebHostEnvironment env)
        {
            _db = db;
            _folder = $@"{env.WebRootPath}\JobRecordSystemAttachment";
        }
        #endregion

        #region 駐廠人員建案系統-Index
        [Authorize]
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

        #region 駐廠人員建案系統-單一Case的詳細內容(Detail)
        [Authorize]
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
                for (var i = 0; i < (CategoryList.Count()); i++)
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

        #region 駐廠人員建案系統-新建案件(Create)
        [HttpGet]
        [Authorize]
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
        [Authorize]
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

        #region 駐廠人員建案系統-編輯案件(Edit)
        [Authorize]
        public async Task<IActionResult> JobRecordsEditCase(int? id, JobRecordsViewModel model, string[] CaseCategory)
        {
            if(id == null)
            {
                return NotFound();
            }

            //撈取案件編號與輸入id相同的案件資料
            var GetJobRecordsSingleData = await _db.JobRecords.FirstOrDefaultAsync(j => j.CaseId == id);

            if (GetJobRecordsSingleData == null)
            {
                return NotFound();
            }

            model.JobRecordsModel = GetJobRecordsSingleData;
            model.LocationList = await (from j in _db.JobRecordsLocationItem select j).ToListAsync();
            model.ProductTypeList = await (from j in _db.JobRecordsProductType select j).ToListAsync();
            model.OSVersionList = await (from j in _db.JobRecordsOSVersion select j).ToListAsync();
            model.CategoryList = await (from j in _db.JobRecordsCategory select j).ToListAsync();

            return View(model);
        }

        [HttpPost, ActionName(nameof(JobRecordsEditCase))]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> JobRecordsEditCaseConfirm(int? id, JobRecordsViewModel model, string[] CaseCategory)
        {
            if (id == null)
            {
                return NotFound();
            }

            model.JobRecordsModel.Category = String.Join(',', CaseCategory);

            if(ModelState.IsValid)
            {
                _db.Update(model.JobRecordsModel);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(JobRecordsSingleCaseDetail), new { id = model.JobRecordsModel.CaseId });
            }

            return View(model.JobRecordsModel);
        }
        #endregion

        #region 駐廠人員建案系統-刪除案件(Delete)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> JobRecordsDeleteCase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var GetRecordFromDB = await _db.JobRecords.FindAsync(id);

            if (GetRecordFromDB == null)
            {
                return NotFound();
            }

            _db.JobRecords.Remove(GetRecordFromDB);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(JobRecordSystemIndex));
        }
        #endregion

        #region 駐廠人員建案系統-關閉案件(CloseCase)
        [Authorize]
        public async Task<IActionResult> CloseCase(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var GetRecordById = await _db.JobRecords.FindAsync(id);

            if(GetRecordById != null)
            {
                JobRecords model = new JobRecords();

                model = GetRecordById;

                model.CaseStatus = 2;

                model.ClosedDate = DateTime.Now;

                model.ClosedOnsiteName = User.Identity.Name.ToString();

                if(ModelState.IsValid)
                {
                    _db.Update(model);
                    await _db.SaveChangesAsync();
                }

                return RedirectToAction(nameof(JobRecordsSingleCaseDetail), new { id = model.CaseId });
            }
            else
            {
                return RedirectToAction(nameof(JobRecordForbiddenPage), new { ErrorCode = 2 });
            }

        }
        #endregion

        #region 駐廠人員建案系統-案件留言(CaseReply)
        [Authorize]
        public async Task<IActionResult> CreateReply(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var CheckCaseIdExist = await _db.JobRecords.FindAsync(id);

            if(CheckCaseIdExist == null)
            {
                return NotFound();
            }
            else
            {
                //案件呈現Closed時
                if(CheckCaseIdExist.CaseStatus == 2)
                {
                    return RedirectToAction(nameof(JobRecordForbiddenPage), new { ErrorCode = 1 });
                }
                else
                {
                    ViewData["PassCaseId"] = id;
                    return View();
                }               
            }
        }

        [HttpPost, ActionName(nameof(CreateReply))]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateReplyConfirm(JobRecordsReply model)
        {
            var GetLastReplyRecord = await _db.JobRecordsReply.OrderBy(j => j.ReplyId).LastOrDefaultAsync();

            if (GetLastReplyRecord == null)
            {
                model.ReplyId = 1;
            }
            else
            {
                model.ReplyId = GetLastReplyRecord.ReplyId + 1;
            }

            model.ReplyDateTime = DateTime.Now;

            if(ModelState.IsValid)
            {
                _db.Add(model);
                await _db.SaveChangesAsync();

                return RedirectToAction("JobRecordsSingleCaseDetail", "JobRecordSystem", new { id = model.RelatedWithJobRecordsId});
            }

            return View(model);
        }
        #endregion

        #region 駐廠人員建案系統-禁止瀏覽頁面(Forbidden)
        //跳轉頁面
        //參考資料(1) : https://stackoverflow.com/questions/24700208/mvc-redirect-to-another-page-after-a-four-second-pause/24700476
        [Authorize]
        public IActionResult JobRecordForbiddenPage(int? ErrorCode, int? id)
        {
            switch(ErrorCode)
            {
                case 1:
                    ViewData["PassErrorMessage"] = "透過URL嘗試訪問已Closed之案件留言是被禁止的";
                    ViewData["PassControllerName"] = "JobRecordSystem";
                    ViewData["PassActionName"] = "JobRecordSystemIndex";
                    break;
                case 2:
                    ViewData["PassErrorMessage"] = "此筆Id不存在";
                    ViewData["PassControllerName"] = "JobRecordSystem";
                    ViewData["PassActionName"] = "JobRecordSystemIndex";
                    break;
                case 3:
                    ViewData["PassErrorMessage"] = "檔案上傳失敗，檔案大小>1MB";
                    ViewData["PassControllerName"] = "JobRecordSystem";
                    ViewData["PassActionName"] = "JobRecordSystemIndex";
                    ViewData["Passid"] = id;
                    break;
                default:
                    ViewData["PassErrorMessage"] = "No Error Message";
                    ViewData["PassControllerName"] = "Home";
                    ViewData["PassActionName"] = "Profile";
                    break;
            }

            return View();
        }
        #endregion

        #region 駐廠人員建案系統-附件上傳
        [HttpPost]
        public async Task<IActionResult> UploadAttachment(int? id, List<IFormFile> files)
        {
            if(id == null)
            {
                return NotFound();
            }

            //計算檔案容量
            var FileSize = files.Sum(f => f.Length);

            if (FileSize > 1024000)
            {
                return RedirectToAction(nameof(JobRecordForbiddenPage), new { ErrorCode = 3, id = id });
            }
            else
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var FileName = file.FileName;
                        //預設路徑為JobRecordSystemAttachment目錄下，然後依當天日期創建資料夾，存放當天所上傳的附件
                        //參考資料 : https://stackoverflow.com/questions/9065598/if-a-folder-does-not-exist-create-it
                        var FileDirectory = $@"{_folder}\{DateTime.Now.ToString("yyyyMMdd")}";
                        var FileStoredPath = $@"{FileDirectory}\{FileName}";

                        //確認路徑是否存在，如果不存在，則create當天日期為名的資料夾，如果已經存在，則直接進行附件上傳
                        bool CheckFilePathExist = Directory.Exists(FileDirectory);

                        if (!CheckFilePathExist)
                        {
                            Directory.CreateDirectory(FileDirectory);
                            using (var stream = new FileStream(FileStoredPath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }
                        else
                        {
                            //如果路徑存在，則確認附件檔的檔名是否
                            bool CheckFileNameExist = System.IO.File.Exists(FileStoredPath);
                            if (!CheckFileNameExist)
                            {
                                using (var stream = new FileStream(FileStoredPath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }
                            }
                            else
                            {
                                var GetFileNameWithNoExt = Path.GetFileNameWithoutExtension(FileStoredPath);
                                var GetFileExtension = Path.GetExtension(FileStoredPath);
                                FileName = $@"{GetFileNameWithNoExt}_Modify_{GetFileExtension}";

                                FileStoredPath = $@"{FileDirectory}\{GetFileNameWithNoExt}_Modify_{GetFileExtension}";

                                using (var stream = new FileStream(FileStoredPath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }
                            }
                        }

                        //新增一則上傳附件的留言到案件中
                        JobRecordsReply model = new JobRecordsReply();

                        var GetLastReplyRecordId = await _db.JobRecordsReply.OrderBy(j => j.ReplyId).LastOrDefaultAsync();

                        model.ReplyId = GetLastReplyRecordId.ReplyId + 1;
                        model.RelatedWithJobRecordsId = Convert.ToInt32(id);
                        model.ReplyPersonName = User.Identity.Name.ToString();
                        model.ReplyDateTime = DateTime.Now;
                        model.ReplyContent = $@"{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")} 上傳附件";
                        model.IsThereAttachment = "1";
                        model.AttachmentName = FileName;

                        _db.Add(model);
                        await _db.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(JobRecordsSingleCaseDetail), new { id = id });
            }
        }
        #endregion

    }
}
