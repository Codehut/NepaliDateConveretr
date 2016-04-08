using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NepaliDateConverter.Helper
{
    public class FullDate
    {
        public Date ConvertedDate { get; set; }
        public string ConvertedDayOfWeek { get; set; }
    }

    public class Date
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
