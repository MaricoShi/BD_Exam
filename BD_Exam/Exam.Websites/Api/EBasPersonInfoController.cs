﻿using Exam.Data;
using Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.OData;

namespace Exam.Websites.Api
{
    public class EBasPersonInfoController : ODataController
    {
        private ExamEntities db = new ExamEntities();

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