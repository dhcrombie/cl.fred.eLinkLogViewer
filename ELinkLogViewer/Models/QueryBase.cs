using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELinkLogViewer.Models.QueryBase
{
    namespace Request
    {
        public class SearchQuery
        {
            public int PageNo { set; get; }
            public int PageSize { set; get; }
        }
    }
    
    namespace Response
    {
        public class QueryResults
        {
            public int TotalResultCount { set; get; }
        }
    }
}
