using System;

namespace FitAppka.Strategy.StrategyImpl.FindDayStrategyImpl
{
    public class DateConverter
    {
        internal static DateTime ConvertToDateTimeFrom(string dateFrom)
        {
            return string.IsNullOrEmpty(dateFrom) ? DateTime.MinValue : Convert.ToDateTime(dateFrom);
        }

        internal static  DateTime ConvertToDateTimeTo(string dateTo)
        {
            return string.IsNullOrEmpty(dateTo) ? DateTime.MaxValue : Convert.ToDateTime(dateTo);
        }
    }
}
