using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Analysis.JsonConverters
{
    public class UnixTimestampConverter : DateTimeConverterBase
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(UnixTimestampFromDateTime((DateTime) value).ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var dt = reader.Value == null ? Epoch : TimeFromUnixTimestamp((long) reader.Value);
            return dt;
        }

        private static DateTime TimeFromUnixTimestamp(long unixTimestamp)
        {
            var timespan = TimeSpan.FromMilliseconds(unixTimestamp);
            var dt = Epoch + timespan;
            return dt;
        }

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            var unixTimestamp = date.Millisecond - Epoch.Millisecond;
            return unixTimestamp;
        }
    }
}