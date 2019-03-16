using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ixf_V2.Support
{
    public class MyPdfActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //Content-Disposition=attachment%3b+filename%3d%22The+Garbage+Collection+Handbook.pdf%22}
            var filerHeader = filterContext.HttpContext.Response.Headers.Get("Content-Disposition");
            if (!string.IsNullOrEmpty(filerHeader) && filerHeader.Substring(0, "attachment".Length).ToLower().Equals("attachment"))
            {
                filterContext.HttpContext.Response.Headers["Content-Disposition"] = "inline" + filerHeader.Substring("attachment".Length, filerHeader.Length - "attachment".Length);
            }
        }
    }

    public class MyActionNameSelecter : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            return actionName.Contains("-PDF文件预览");
        }
    }
}