using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Presentation.Common.Models.Json
{
    public class CommonJsonResult : JsonResult
    {
        private const string OperationsIsNotAllowedForGetRequestsMsg = "Operations is not allowed for GET requests.";
        private const string ControllerContextIsNullMsg = "Controller context is null.";
        private const string JsonContentType = "application/json";
        private const string GetHttpMethod = "GET";

        private readonly IEnumerable<JsonConverter> jsonConverters;

        public CommonJsonResult(BaseJsonResponse data, string contentType, Encoding contentEncoding,
            JsonRequestBehavior behavior, IEnumerable<JsonConverter> jsonConverters)
        {
            this.Data = data;
            this.ContentEncoding = contentEncoding;
            this.ContentType = contentType;
            this.jsonConverters = jsonConverters;
            this.JsonRequestBehavior = behavior;
            this.MaxJsonLength = null;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(ControllerContextIsNullMsg);
            }
            if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet) &&
                string.Equals(context.HttpContext.Request.HttpMethod, GetHttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(OperationsIsNotAllowedForGetRequestsMsg);
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(this.ContentType) ? this.ContentType : JsonContentType;
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                var serializer = new JsonSerializer();
                if (this.RecursionLimit.HasValue)
                {
                    serializer.MaxDepth = this.RecursionLimit.Value;
                }

                foreach (JsonConverter jsonConverter in jsonConverters)
                {
                    serializer.Converters.Add(jsonConverter);
                }

                serializer.Serialize(response.Output, this.Data);
            }
        }

    }
}