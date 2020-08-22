using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Analysis.JsonConverters
{
    public class MillisecondsTimeSpanConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(MillisecondsFromTimeSpan((TimeSpan) value).ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var dt = reader.Value == null ? new TimeSpan() : TimeSpanFromMilliseconds((long) reader.Value);
            return dt;
        }

        private static TimeSpan TimeSpanFromMilliseconds(long milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        public static long MillisecondsFromTimeSpan(TimeSpan timeSpan)
        {
            return (long) timeSpan.TotalMilliseconds;
        }
    }
}