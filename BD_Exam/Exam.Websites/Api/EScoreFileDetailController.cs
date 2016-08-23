using Exam.Data;
using Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.OData;

namespace Exam.Websites.Api
{
    public class EScoreFileDetailController : ODataController
    {
        private ExamEntities db = new ExamEntities();

        public IQueryable<EScoreFileDetail> Get()
        {
            return db.EScoreFileDetail;
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