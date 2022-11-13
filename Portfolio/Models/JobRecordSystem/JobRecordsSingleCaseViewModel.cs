using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.JobRecordSystem
{
    public class JobRecordsSingleCaseViewModel
    {
        public JobRecords JobRecordsModel { get; set; }

        public List<JobRecordsReply> JobRecordsReplyModel { get; set; }

        public IEnumerable<JobRecords> JobRecordsEnum { get; set; }

        #region Load Item into List
        public List<JobRecordsCaseStatusItem> CaseStatusList { get; set; }
        public List<JobRecordsLocationItem> LocationList { get; set; }
        public List<JobRecordsProductType> ProductTypeList { get; set; }
        public List<JobRecordsOSVersion> OSVersionList { get; set; }
        public List<JobRecordsCategory> CategoryList { get; set; }
        #endregion
    }
}
