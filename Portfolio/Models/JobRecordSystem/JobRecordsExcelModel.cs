using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.JobRecordSystem
{
    public class JobRecordsExcelModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string BuildDate { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string UserName { get; set; }
        public string OnsiteName { get; set; }
        public string HostName { get; set; }
        public string Product { get; set; }
        public string OS { get; set; }
        public string Category { get; set; }
        public string ClosedDate { get; set; }
        public string CloseOnsiteName { get; set; }
    }
}
