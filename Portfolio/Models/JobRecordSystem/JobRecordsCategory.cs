﻿using System;
using System.Collections.Generic;

namespace Portfolio.Models.JobRecordSystem
{
    public partial class JobRecordsCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CategorySort { get; set; }
    }
}
