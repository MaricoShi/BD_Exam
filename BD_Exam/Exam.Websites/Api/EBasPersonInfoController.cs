using Exam.Data;
using Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Data.Entity;

namespace Exam.Websites.Api
{
    public class EBasPersonInfoController : ODataController
    {
        private ExamEntities db = new ExamEntities();
        [Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<EBasPersonInfo> Get()
        {
            return db.EBasPersonInfo;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}