using System;

namespace NeatCoin
{
    internal static class DateTimeOffsetExtensions
    {
        internal static string AsString(this DateTimeOffset dateTimeOffset) => dateTimeOffset.ToString("o");
    }
}