using Exam.Data;
using Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.OData;

namespace Exam.Websites.Api
{
    public class EConfigProfessionProjectController : ODataController
    {
        private ExamEntities db = new ExamEntities();

        public IQueryable<EConfigProfessionProject> Get()
        {
            return db.EConfigProfessionProject;
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