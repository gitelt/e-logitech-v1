﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ELT.COMMON;
using ELT.CDT;
using System.Collections;
using DevExpress.Web.Mvc;

namespace ELT.WEB.Models
{
    [Serializable]
    public class OceanReportDetailModel
    {
        public string Key { get; set; }
        public string KeyFieldName { get; set; }
        public List<OceanTransactionItem> Elements { get; set; }
        public GridViewSettings Settings { get; set; }
    }
}