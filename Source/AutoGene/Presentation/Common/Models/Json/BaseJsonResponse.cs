using System;
using System.Collections.Generic;

namespace Presentation.Common.Models.Json
{
    public class BaseJsonResponse
    {
        public JsonResponseResult Code
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public IEnumerable<JsonResultMessage> Messages
        {
            get;
            set;
        }

        public object Data
        {
            get;
            set;
        }
    }
}