using Exam.Data.Models;
using Microsoft.Data.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.OData.Builder;


namespace Exam.Websites
{
    public class ODataConfig
    {
        public static IEdmModel GetEdmModel()
        {

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "Odata";
            builder.ContainerName = "DefaultContainer";

            builder.EntitySet<ESys>("ESys");

            return builder.GetEdmModel();

        }
    }
}