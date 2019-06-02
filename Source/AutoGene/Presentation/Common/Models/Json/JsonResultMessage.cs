namespace Presentation.Common.Models.Json
{
    public class JsonResultMessage
    {
        /// <summary>
        /// Name of the field (property) associated with this message.
        /// </summary>
        public string FieldName
        {
            get;
            set;
        }

        /// <summary>
        /// Equals to state of the current message.
        /// </summary>
        public JsonResultMessageState State
        {
            get;
            set;
        }

        /// <summary>
        /// Localized result message for specified field name.
        /// </summary>
        public string Message
        {
            get;
            set;
        }
    }
}