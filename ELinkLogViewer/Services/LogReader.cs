using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELinkLogViewer.Services
{
    public static class LogReader
    {
        public static Models.LogItems.Result.GetLogItems GetLogItems(Models.LogItems.Query.GetLogItems Query)
        {
            Models.LogItems.Result.GetLogItems Result = new Models.LogItems.Result.GetLogItems();
            using (eLinkLogEntities db = new eLinkLogEntities())
            {
                var DBQuery = (from LogItemX in db.Logs
                               join LogItemResponseX in db.Logs on new { LogItemX.filename, type = "response" } equals new { LogItemResponseX.filename, type = LogItemResponseX.type.ToLower() }
                               where LogItemX.type.ToLower() == "request"
                                  && System.Data.Entity.DbFunctions.TruncateTime(LogItemX.timestamp) >= System.Data.Entity.DbFunctions.TruncateTime(Query.Filters.DateFrom)
                                  && System.Data.Entity.DbFunctions.TruncateTime(LogItemX.timestamp) <= System.Data.Entity.DbFunctions.TruncateTime(Query.Filters.DateTo)
                               select new { Request = LogItemX, Response = LogItemResponseX });
                
                // Filter result set.
                if (!string.IsNullOrWhiteSpace(Query.Filters.Cover))
                {
                    DBQuery = DBQuery.Where(LogItemX => LogItemX.Request.cover.Contains(Query.Filters.Cover)
                                                     || LogItemX.Response.cover.Contains(Query.Filters.Cover));
                }
                if (!string.IsNullOrWhiteSpace(Query.Filters.Branch))
                {
                    DBQuery = DBQuery.Where(LogItemX => LogItemX.Request.branch.Contains(Query.Filters.Branch)
                                                     || LogItemX.Response.branch.Contains(Query.Filters.Branch));
                }
                if (!string.IsNullOrWhiteSpace(Query.Filters.Client))
                {
                    DBQuery = DBQuery.Where(LogItemX => LogItemX.Request.client.Contains(Query.Filters.Client)
                                                     || LogItemX.Response.client.Contains(Query.Filters.Client));
                }
                if (!string.IsNullOrWhiteSpace(Query.Filters.ClientNumber))
                {
                    DBQuery = DBQuery.Where(LogItemX => LogItemX.Request.clientnumber.Contains(Query.Filters.ClientNumber)
                                                     || LogItemX.Response.clientnumber.Contains(Query.Filters.ClientNumber));
                }
                if (!string.IsNullOrWhiteSpace(Query.Filters.FileContents))
                {
                    DBQuery = DBQuery.Where(LogItemX => LogItemX.Request.filecontents.Contains(Query.Filters.FileContents)
                                                     || LogItemX.Response.filecontents.Contains(Query.Filters.FileContents));
                }
                
                var FilteredQuery = DBQuery.Select(LogItemX => new Models.LogItems.Result.LogEntryPair
                                                               {
                                                                    Request = new Models.LogItems.Result.BasicDetails
                                                                              {
                                                                                  Id = LogItemX.Request.Id,
                                                                                  Cover = LogItemX.Request.cover,
                                                                                  Branch = LogItemX.Request.branch,
                                                                                  Client = LogItemX.Request.client,
                                                                                  ClientNumber = LogItemX.Request.clientnumber,
                                                                                  Invoice = LogItemX.Request.invoice,
                                                                                  ClientTransaction = LogItemX.Request.clienttransaction,
                                                                                  PolicyTransaction = LogItemX.Request.policytransaction,
                                                                                  Version = LogItemX.Request.version,
                                                                                  //FileContents = LogItemX.Request.filecontents,
                                                                                  //Filename = LogItemX.Request.filename,
                                                                                  Timestamp = LogItemX.Request.timestamp,
                                                                                  //Type = LogItemX.Request.type,
                                                                                  Status = LogItemX.Request.status
                                                                              },
                                                                    Response = new Models.LogItems.Result.BasicDetails
                                                                               {
                                                                                  Id = LogItemX.Response.Id,
                                                                                  Cover = LogItemX.Response.cover,
                                                                                  Branch = LogItemX.Response.branch,
                                                                                  Client = LogItemX.Response.client,
                                                                                  ClientNumber = LogItemX.Response.clientnumber,
                                                                                  Invoice = LogItemX.Response.invoice,
                                                                                  ClientTransaction = LogItemX.Response.clienttransaction,
                                                                                  PolicyTransaction = LogItemX.Response.policytransaction,
                                                                                  Version = LogItemX.Response.version,
                                                                                  //FileContents = LogItemX.Response.filecontents,
                                                                                  //Filename = LogItemX.Response.filename,
                                                                                  Timestamp = LogItemX.Response.timestamp,
                                                                                  //Type = LogItemX.Response.type,
                                                                                  Status = LogItemX.Response.status
                                                                               }
                                                               });

                // Sort result set.
                FilteredQuery = FilteredQuery.OrderByDescending(LogItemX => LogItemX.Request.Timestamp);
                
                Result.TotalResultCount = FilteredQuery.Count();
                
                // Page results?
                if (Query.PageNo > 0 && Query.PageSize > 0)
                {
                    Result.Results = FilteredQuery.Skip(Query.PageSize * (Query.PageNo - 1)).Take(Query.PageSize).ToList();
                }
                else
                {
                    Result.Results = FilteredQuery.ToList();
                }
            }
            return (Result);
        }
        
        public static Models.LogItems.Result.LogEntryPair GetLogItem(int LogItemId)
        {
            Models.LogItems.Result.LogEntryPair Result = null;
            using (eLinkLogEntities db = new eLinkLogEntities())
            {
                Result = (from LogItemX in db.Logs
                          join LogItemResponseX in db.Logs on new { LogItemX.filename, type = "response" } equals new { LogItemResponseX.filename, type = LogItemResponseX.type.ToLower() }
                          where LogItemX.Id == LogItemId
                          select new Models.LogItems.Result.LogEntryPair
                                 {
                                    Request = new Models.LogItems.Result.FullDetails
                                              {
                                                  Id = LogItemX.Id,
                                                  Cover = LogItemX.cover,
                                                  Branch = LogItemX.branch,
                                                  Client = LogItemX.client,
                                                  ClientNumber = LogItemX.clientnumber,
                                                  Invoice = LogItemX.invoice,
                                                  ClientTransaction = LogItemX.clienttransaction,
                                                  PolicyTransaction = LogItemX.policytransaction,
                                                  Version = LogItemX.version,
                                                  FileContents = LogItemX.filecontents,
                                                  Filename = LogItemX.filename,
                                                  Timestamp = LogItemX.timestamp,
                                                  Type = LogItemX.type,
                                                  Status = LogItemX.status
                                              },
                                    Response = new Models.LogItems.Result.FullDetails
                                               {
                                                   Id = LogItemResponseX.Id,
                                                   Cover = LogItemResponseX.cover,
                                                   Branch = LogItemResponseX.branch,
                                                   Client = LogItemResponseX.client,
                                                   ClientNumber = LogItemResponseX.clientnumber,
                                                   Invoice = LogItemResponseX.invoice,
                                                   ClientTransaction = LogItemResponseX.clienttransaction,
                                                   PolicyTransaction = LogItemResponseX.policytransaction,
                                                   Version = LogItemResponseX.version,
                                                   FileContents = LogItemResponseX.filecontents,
                                                   Filename = LogItemResponseX.filename,
                                                   Timestamp = LogItemResponseX.timestamp,
                                                   Type = LogItemResponseX.type,
                                                   Status = LogItemResponseX.status
                                               }
                                 }).SingleOrDefault();
            }
            return (Result);
        }
    }
}
