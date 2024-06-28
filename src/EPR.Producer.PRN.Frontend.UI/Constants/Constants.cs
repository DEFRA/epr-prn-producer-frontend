#pragma warning disable SA1649 // FileNameMustMatchTypeName
#pragma warning disable SA1402 // File may only contain a single type

namespace EPR.Producer.PRN.Frontend.Constants
{
    using System.Globalization;

    public static class CultureConstants
    {
        public static readonly CultureInfo English = new("en-GB");
        public static readonly CultureInfo Welsh = new("cy-GB");
    }
}
#pragma warning restore SA1402 // File may only contain a single type
#pragma warning restore SA1649 // Restore StyleCop warnings