using System;

namespace FitAppka.Strategy.ChartTypeStrategyImpl
{
    public class DateConverter
    {
        internal static DateTime ConvertToDateTimeAndPreventNull(string date, bool fromOrTo)
        {
            if (date == null) {
                return fromOrTo ? DateTime.Now.AddDays(-7).Date : DateTime.Now.Date;
            }

            return Convert.ToDateTime(date).Date;
        }

        internal static string ConvertToJSDate(string date, bool fromOrTo)
        {
            return ConvertToDateTimeAndPreventNull(date, fromOrTo).ToString("dd.MM.yyyy");
        }
    }
}
