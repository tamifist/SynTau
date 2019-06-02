using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Mobile.Backend.ActionFilters
{
    public class QueryableExpandAttribute : ActionFilterAttribute
    {
        private const string ODataExpandOption = "$expand=";

        public QueryableExpandAttribute(string expand)
        {
            this.AlwaysExpand = expand;
        }

        public string AlwaysExpand { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            var parts = new List<string>();

            if (!string.IsNullOrEmpty(request.RequestUri.Query))
            {
                string query = request.RequestUri.Query.Substring(1);
                parts = query.Split('&').ToList();
                bool foundExpand = false;

                for (int i = 0; i < parts.Count; i++)
                {
                    string segment = parts[i];
                    if (segment.StartsWith(ODataExpandOption, StringComparison.Ordinal))
                    {
                        foundExpand = true;
                        parts[i] += "," + this.AlwaysExpand;
                        break;
                    }
                }

                if (!foundExpand)
                    parts.Add(ODataExpandOption + this.AlwaysExpand);
            }

            var modifiedRequestUri = new UriBuilder(request.RequestUri);
            modifiedRequestUri.Query = string.Join("&",
                parts.Where(p => p.Length > 0));

            request.RequestUri = modifiedRequestUri.Uri;
            base.OnActionExecuting(actionContext);
        }
    }
}