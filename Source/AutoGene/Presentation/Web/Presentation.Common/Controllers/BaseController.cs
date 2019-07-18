using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.Common.Json;

namespace Presentation.Common.Controllers
{
    public abstract class BaseController : Controller
    {
        private IEnumerable<JsonConverter> jsonConverters = DefaultJsonConverters.Instance;

        protected internal IEnumerable<JsonConverter> JsonConverters
        {
            get
            {
                return jsonConverters;
            }
            set
            {
                jsonConverters = value;
            }
        }

        public override JsonResult Json(object data)
        {
//            var response = data as BaseJsonResponse;
//            if (response == null)
//            {
//                response = CreateEmptyJsonResponse<BaseJsonResponse>(JsonResponseResult.Success);
//                response.Data = data;
//            }

            return new CommonJsonResult(data, JsonConverters);
        }

        protected JsonResult JsonSuccess(string id)
        {
            var response = CreateEmptyJsonResponse<BaseJsonResponse>(JsonResponseResult.Success, id);
            return Json(response);
        }

        protected JsonResult JsonError(string message = "")
        {
            var response = CreateEmptyJsonResponse<BaseJsonResponse>(JsonResponseResult.Error, message);
            return Json(response);
        }

        protected T CreateEmptyJsonResponse<T>(JsonResponseResult result, string id = "")
            where T : BaseJsonResponse, new()
        {
            var jsonResponse = new T
            {
                Data = id,
                Code = result
            };
            return jsonResponse;
        }
    }
}
