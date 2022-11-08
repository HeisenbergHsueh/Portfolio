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
    }
}
