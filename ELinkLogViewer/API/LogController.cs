using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ELinkLogViewer.API
{
    public class LogController : ApiController
    {
        public IHttpActionResult Post(Models.LogItems.Query.GetLogItems Query)
        {
            if (ModelState.IsValid)
            {
                Models.LogItems.Result.GetLogItems Result = Services.LogReader.GetLogItems(Query);
                return Ok(Result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
