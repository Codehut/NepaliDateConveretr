using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NepaliDateConverter.Tests
{
    [TestClass()]
    public class DateConverterTests
    {
        [TestMethod()]
        public void EngToNepTest()
        {
            DateTime expecteddt = DateTime.Parse("2050-06-14");

            var convertedDate = new DateConverter().EngToNep(1993, 9, 30);

            var actualDate = DateTime.Parse(convertedDate.ConvertedDate.Year.ToString() + "-" + convertedDate.ConvertedDate.Month.ToString() + "-" + convertedDate.ConvertedDate.Day.ToString());


            Assert.AreEqual(expecteddt, actualDate, "Verified");
        }

        [TestMethod()]
        public void NepToEngTest()
        {
            DateTime expecteddt = DateTime.Parse("1993-09-30");

            var convertedDate = new DateConverter().NepToEng(2050, 6, 14);

            var actualDate = DateTime.Parse(convertedDate.ConvertedDate.Year.ToString() + "-" + convertedDate.ConvertedDate.Month.ToString() + "-" + convertedDate.ConvertedDate.Day.ToString());


            Assert.AreEqual(expecteddt, actualDate, "Verified");
        }
    }
}