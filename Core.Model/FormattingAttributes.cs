using System;
using System.Collections.Generic;

namespace Core.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class Format : Attribute
    {
        private Dictionary<FormatType, string> formats = new Dictionary<FormatType, string>()
        {
            { FormatType.IntegralNumber, "" },
            { FormatType.Numeric0, "N0" },
            { FormatType.Numeric2, "N2" },
            { FormatType.Numeric4, "N4" },
            { FormatType.Currency2, "C2" },
            { FormatType.Currency4, "C4" },
            { FormatType.Date, "d" },
            { FormatType.Time, "t" }
        };
        public FormatType FormatType { get; }

        public Format(FormatType _format)
        {
            FormatType = _format;
        }

        public string GetFormat()
        {
            return formats[FormatType];
        }

        public string GetFormatedString(dynamic objectToFormat)
        {
            if (objectToFormat == null) return string.Empty;

            return objectToFormat.ToString(GetFormat());
        }
    }

    public enum FormatType
    {
        IntegralNumber,
        Numeric0,
        Numeric2,
        Numeric4,
        Currency2,
        Currency4,
        Date,
        Time
    }
}
