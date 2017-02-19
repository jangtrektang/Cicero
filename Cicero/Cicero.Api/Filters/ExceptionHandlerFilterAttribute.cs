using Cicero.Core.Helpers.ErrorLogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Cicero.Api.Filters
{
    public class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            ErrorHelper.LogException(actionExecutedContext.Exception, "");
        }
    }
}