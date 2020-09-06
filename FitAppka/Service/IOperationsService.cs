using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAppka.Service
{
    public interface IOperationsService
    {
        public decimal Round(double number);
        public double RoundDouble(double? number);
        public double SumAllListItems(List<double> list);
    }
}
