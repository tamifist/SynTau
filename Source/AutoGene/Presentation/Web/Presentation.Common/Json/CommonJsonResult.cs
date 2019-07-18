using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Presentation.Common.Json
{
    public class CommonJsonResult : JsonResult
    {
        private const string ControllerContextIsNullMsg = "Controller context is null.";
        private const string JsonContentType = "application/json";

        private readonly IEnumerable<JsonConverter> jsonConverters;

        public CommonJsonResult(object value, IEnumerable<JsonConverter> jsonConverters) : base(value)
        {
            this.jsonConverters = jsonConverters;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            await base.ExecuteResultAsync(context);

            if (context == null)
            {
                throw new ArgumentNullException(ControllerContextIsNullMsg);
            }
            //            if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet) &&
            //                string.Equals(context.HttpContext.Request.HttpMethod, GetHttpMethod, StringComparison.OrdinalIgnoreCase))
            //            {
            //                throw new InvalidOperationException(OperationsIsNotAllowedForGetRequestsMsg);
            //            }
            HttpResponse response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : JsonContentType;
            //            if (this.ContentEncoding != null)
            //            {
            //                response.ContentEncoding = this.ContentEncoding;
            //            }
            if (Value != null)
            {
                var serializer = new JsonSerializer();
                //                if (this.RecursionLimit.HasValue)
                //                {
                //                    serializer.MaxDepth = this.RecursionLimit.Value;
                //                }

                foreach (JsonConverter jsonConverter in jsonConverters)
                {
                    serializer.Converters.Add(jsonConverter);
                }

                using (var writer = new StreamWriter(response.Body))
                {
                    new JsonSerializer().Serialize(writer, Value);
                    await writer.FlushAsync().ConfigureAwait(false);
                }
            }
        }
    }
}