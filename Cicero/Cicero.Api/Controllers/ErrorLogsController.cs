using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Cicero.Core.DataObjects;

namespace Cicero.Api.Controllers
{
    public class ErrorLogsController : ApiController
    {
        private CiceroContext db = new CiceroContext();

        // GET: api/ErrorLogs
        public IQueryable<ErrorLog> GetErrorLogs()
        {
            return db.ErrorLogs;
        }

        // GET: api/ErrorLogs/5
        [ResponseType(typeof(ErrorLog))]
        public async Task<IHttpActionResult> GetErrorLog(int id)
        {
            ErrorLog errorLog = await db.ErrorLogs.FindAsync(id);
            if (errorLog == null)
            {
                return NotFound();
            }

            return Ok(errorLog);
        }       

        // DELETE: api/ErrorLogs/5
        [ResponseType(typeof(ErrorLog))]
        public async Task<IHttpActionResult> DeleteErrorLog(int id)
        {
            ErrorLog errorLog = await db.ErrorLogs.FindAsync(id);
            if (errorLog == null)
            {
                return NotFound();
            }

            db.ErrorLogs.Remove(errorLog);
            await db.SaveChangesAsync();

            return Ok(errorLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ErrorLogExists(int id)
        {
            return db.ErrorLogs.Count(e => e.ID == id) > 0;
        }
    }
}