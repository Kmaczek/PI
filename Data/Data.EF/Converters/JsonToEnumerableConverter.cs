using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Text.Json;

namespace Data.EF.Converters
{
    internal class JsonToEnumerableConverter<T> : ValueConverter<IEnumerable<T>, string>
    {
        public JsonToEnumerableConverter()
        : base(
              v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
              v => JsonSerializer.Deserialize<IEnumerable<T>>(v, (JsonSerializerOptions)null))
        { }
    }
}
