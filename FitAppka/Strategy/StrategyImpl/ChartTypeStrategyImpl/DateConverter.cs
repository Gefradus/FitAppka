using FitAppka.Models;
using System;
using System.Collections.Generic;

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

        internal static List<MeasurementDTO> SortByMeasurementDate(List<MeasurementDTO> list)
        {
            list.Sort((x, y) => x.DateOfMeasurement.CompareTo(y.DateOfMeasurement));
            return list;
        }

        internal static List<ChartDataInDayDTO> SortChartDataByDate(List<ChartDataInDayDTO> list)
        {
            list.Sort((x, y) => x.DateOfDay.CompareTo(y.DateOfDay));
            return list;
        }

    }
}
