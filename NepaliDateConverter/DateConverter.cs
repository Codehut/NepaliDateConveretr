using System;
using System.Collections.Generic;
using NepaliDateConverter.Helper;

namespace NepaliDateConverter
{
    public class DateConverter
    {
        #region Normal Variables
        private Dictionary<int, int[]> dates = new Dictionary<int, int[]>();
        #endregion

        #region ENG2NEP VARIABLES
        //least possible English date
        private readonly int StartingEngYear = 1944;
        private readonly int StartingEngMonth = 01;
        private readonly int StartingEngDay = 01;

        //equivalent Nepali date
        private readonly int StartingNepYear = 2000;
        private readonly int StartingNepMonth = 9;
        private readonly int StartingNepDay = 17;
        int daysInMonth = 0;
        int DayofWeek = 7; // 1944/1/1 is saturday
        #endregion

        #region NEP2ENG VARIABLES
        //least possible nepali date
        private readonly int StartNepYear = 2000;
        private readonly int StartNepMonth = 1;
        private readonly int StartNepDay = 1;

        //equivalent english date
        private readonly int StartEngYear = 1943;
        private readonly int StartEngMonth = 4;
        private readonly int StartEngDay = 14;
        long totalNepDaysCount = 0;
        int NepDayofWeek = 4;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DateConverter()
        {
            //Getting the Loaded Dates from DateLoader Class of helper folder
            dates = new DateLoader().LoadDate();
        }
        /// <summary>
        /// Convert english(AD) date to equivalent Nepali(BS) date.
        /// </summary>
        /// <param name="year">English year(Least Valid year is 1944)</param>
        /// <param name="month">Month</param>
        /// <param name="day">Day</param>
        /// <returns>Return Converted Nepali Date.</returns>
        public FullDate EngToNep(int year, int month, int day)
        {
            try
            {
                //AD 2 BS convert logic
                string DayName = string.Empty;
                DayofWeek = 7;
                string date = year + "-" + month + "-" + day;
                var totalEngDays = EngDaysBetween(DateTime.Parse("1944-01-01"), DateTime.Parse(date)); //1944-01-01 is least valid english date

                int nepYear = this.StartingNepYear;
                int nepMonth = this.StartingNepMonth;
                int nepDay = this.StartingNepDay;

                while (totalEngDays != 0)
                {
                    if (dates.ContainsKey(nepYear))
                        daysInMonth = dates[nepYear][nepMonth];
                    nepDay++;

                    if (nepDay > daysInMonth)
                    {
                        nepMonth++;
                        nepDay = 1;
                    }

                    if (nepMonth > 12)
                    {
                        nepYear++;
                        nepMonth = 1;
                    }

                    DayofWeek++;
                    if (DayofWeek > 7)
                        DayofWeek = 1;

                    totalEngDays--;
                }
                switch (DayofWeek)
                {
                    case 1:
                        DayName = "Sunday";
                        break;
                    case 2:
                        DayName = "Monday";
                        break;
                    case 3:
                        DayName = "Tuesday";
                        break;
                    case 4:
                        DayName = "Wednesday";
                        break;
                    case 5:
                        DayName = "Thursday";
                        break;
                    case 6:
                        DayName = "Friday";
                        break;
                    case 7:
                        DayName = "Saturday";
                        break;
                    default:
                        break;

                }
                return new FullDate() { ConvertedDate = new Date { Year = nepYear, Month = nepMonth, Day = nepDay }, ConvertedDayOfWeek = DayName };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Convert Nepali(BS) date to equivalent English(AD) Date
        /// </summary>
        /// <param name="year">Nepali year (2000 is least supported)</param>
        /// <param name="month">Month</param>
        /// <param name="day">Day</param>
        /// <returns>Returns Converted English Date.</returns>
        public FullDate NepToEng(int year, int month, int day)
        {
            //BS 2 AD convert logic
            try
            {
                string DayName = string.Empty;
                NepDayofWeek = 4;
                string date = year + "-" + month + "-" + day;
                //totalNepDaysCount = NepDaysBetween(DateTime.Parse("2000-01-01"), DateTime.Parse(date));
                totalNepDaysCount = NepDaysBetween(DateTime.Parse("2000-01-01"), date);
                #region diff no of days for leap year and non leap year
                int[] daysInMonth = new int[] { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                int[] daysInMonthOfLeapYear = new int[] { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                #endregion

                int engYear = StartEngYear;
                int engMonth = StartEngMonth;
                int engDay = StartEngDay;

                int endDayofMonth = 0;
                //main engine 
                while (totalNepDaysCount != 0)
                {
                    if (IsLeapYear(engYear))
                        endDayofMonth = daysInMonthOfLeapYear[engMonth];
                    else
                        endDayofMonth = daysInMonth[engMonth];

                    engDay++;
                    NepDayofWeek++;
                    if (engDay > endDayofMonth)
                    {
                        engMonth++;
                        engDay = 1;
                        if (engMonth > 12)
                        {
                            engYear++;
                            engMonth = 1;
                        }
                    }

                    if (NepDayofWeek > 7)
                    {
                        NepDayofWeek = 1;
                    }
                    switch (NepDayofWeek)
                    {
                        case 1:
                            DayName = "Sunday";
                            break;
                        case 2:
                            DayName = "Monday";
                            break;
                        case 3:
                            DayName = "Tuesday";
                            break;
                        case 4:
                            DayName = "Wednesday";
                            break;
                        case 5:
                            DayName = "Thursday";
                            break;
                        case 6:
                            DayName = "Friday";
                            break;
                        case 7:
                            DayName = "Saturday";
                            break;
                        default:
                            break;
                    }
                    totalNepDaysCount--;
                }
                return new FullDate() { ConvertedDate = new Date { Year = engYear, Month = engMonth, Day = engDay }, ConvertedDayOfWeek = DayName };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get the total number of days between English dates.
        /// </summary>
        /// <param name="basedate">Base/Starting Date i.e.1944-01-01 (Least Valid English date)</param>
        /// <param name="currentdate">Current date</param>
        private long EngDaysBetween(DateTime basedate, DateTime currentdate)
        {
            long TotalDiffDays = 0;

            while (DateTime.Compare(basedate, currentdate) != 0)
            {
                basedate = basedate.AddDays(1);
                TotalDiffDays++;
            }
            return TotalDiffDays;
        }

        /// <summary>
        /// Get the total number of days between Nepali dates
        /// </summary>
        ///<param name="basedate">Base/Starting Date</param>
        /// <param name="currentdate">Current date</param>
        private long NepDaysBetween(DateTime basedate, string date)
        {
            string[] mydate = date.Split('-');
            long TotalDiffDays = 0;
            //if(Convert.ToInt32(dates[2]) > 32)
            //{

            //}
            for (int i = basedate.Year; i < Convert.ToInt32(mydate[0]); i++)
            {
                for (var j = 1; j <= 12; j++)
                {
                    TotalDiffDays += dates[i][j]; // i year ko j month ko date
                }
            }

            for (var j = basedate.Month; j < Convert.ToInt32(mydate[1]); j++)
            {
                TotalDiffDays += dates[Convert.ToInt32(mydate[0])][j];
            }

            TotalDiffDays += Convert.ToInt32(mydate[2]) - basedate.Day;

            return TotalDiffDays;
        }

        /// <summary>
        /// Check weather given year is leap year or not.
        /// </summary>
        /// <param name="year">Year to check weather leap year or not.</param>
        private Boolean IsLeapYear(int year)
        {
            if (year % 100 == 0)
                return year % 400 == 0;
            else
                return year % 4 == 0;
        }
    }
}
