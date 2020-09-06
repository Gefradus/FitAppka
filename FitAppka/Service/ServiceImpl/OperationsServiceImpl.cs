using System;
using System.Collections.Generic;


namespace FitAppka.Service.ServiceImpl
{
    public class OperationsServiceImpl : IOperationsService
    {
        public decimal Round(double number)
        {
            return Math.Round((decimal)number, 0, MidpointRounding.AwayFromZero);
        }

        public double RoundDouble(double? number)
        {
            return (double) Math.Round((decimal)number, 1, MidpointRounding.AwayFromZero);
        }

        public double SumAllListItems(List<double> list)
        {
            double var = 0;
            foreach (var item in list) { var += item; }
            return var;
        }
    }
}
