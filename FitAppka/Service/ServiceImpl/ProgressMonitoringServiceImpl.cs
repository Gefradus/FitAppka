using FitAppka.Models;
using FitAppka.Repository;
using System;
using System.Collections.Generic;


namespace FitAppka.Service.ServiceImpl
{
    public class ProgressMonitoringServiceImpl : IProgressMonitoringService
    {
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;

        public ProgressMonitoringServiceImpl(IWeightMeasurementRepository weightMeasurementRepository)
        {
            _weightMeasurementRepository = weightMeasurementRepository;
        }

        public List<WeightMeasurement> GetWeightMeasurementListFromTo(string dateFrom, string dateTo)
        {
            var dateTimeFrom = ConvertToDateTimeAndPreventNull(dateFrom, true);
            var dateTimeTo = ConvertToDateTimeAndPreventNull(dateTo, false);
            var list = new List<WeightMeasurement>();

            foreach(var measurement in _weightMeasurementRepository.GetLoggedInClientWeightMeasurements())
            {
                if(measurement.DateOfMeasurement.GetValueOrDefault().Date <= dateTimeTo && measurement.DateOfMeasurement.GetValueOrDefault().Date >= dateTimeFrom)
                {
                    list.Add(measurement);
                }
            }
            return list;
        }

        private DateTime ConvertToDateTimeAndPreventNull(string date, bool fromOrTo)
        {
            if(date == null) {
                return fromOrTo ? DateTime.Now.AddDays(-7) : DateTime.Now;
            }

            return Convert.ToDateTime(date);
        }

        public string ConvertToJSDate(string date, bool fromOrTo)
        {
            return ConvertToDateTimeAndPreventNull(date, fromOrTo).ToString("dd.MM.yyyy");
        }
    }
}
