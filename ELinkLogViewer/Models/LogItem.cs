using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELinkLogViewer.Models.LogItems
{
    namespace Query
    {
        public class GetLogItems : Models.QueryBase.Request.SearchQuery
        {
            public Filters Filters { set; get; }
        }
        
        public class Filters
        {
            public DateTime DateFrom { set; get; }
            public DateTime DateTo { set; get; }
            public string Cover { set; get; }
            public string Branch { set; get; }
            public string Client { set; get; }
            public string ClientNumber { set; get; }
            public string FileContents { set; get; }
        }
    }

    namespace Result
    {
        public class BasicDetails
        {
            public long Id { set; get; }
            public string Cover { set; get; }
            public string Branch { set; get; }
            public string Client { set; get; }
            public string ClientNumber { set; get; }
            public string Invoice { set; get; }
            public string ClientTransaction { set; get; }
            public string PolicyTransaction { set; get; }
            public string Version { set; get; }
            public DateTime Timestamp { set; get; }
            public string Status { set; get; }
        }

        public class FullDetails : BasicDetails
        {
            public string FileContents { set; get; }
            public string Filename { set; get; }
            public string Type { set; get; }
        }

        public class LogEntryPair
        {
            public BasicDetails Request { set; get; }
            public BasicDetails Response { set; get; }
        }

        public class GetLogItems : Models.QueryBase.Response.QueryResults
        {
            public IEnumerable<LogEntryPair> Results { set; get; }
        }
    }
}
