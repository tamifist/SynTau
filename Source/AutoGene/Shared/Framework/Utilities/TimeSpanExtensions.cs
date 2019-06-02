using System;

namespace Shared.Framework.Utilities
{
    public static class TimeSpanExtensions
    {
        public static DateTimeOffset ToDateTimeOffset(this TimeSpan timeSpan)
        {
            return new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeSpan.Zero).Add(timeSpan);
        }
    }
}
