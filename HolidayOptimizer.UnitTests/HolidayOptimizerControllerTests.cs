using HolidayOptimizer.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace HolidayOptimizer.UnitTests
{
    [TestFixture]
    public class HolidayOptimizerControllerTests
    {
        private HolidayOptimizerController _holidayOptimizerController;

        [SetUp]
        public void Setup()
        {
            _holidayOptimizerController = new HolidayOptimizerController();
        }

        [Test]
        public void GetCountryWithMostHolidays_WhenCalled_ReturnString()
        {
            var result = _holidayOptimizerController.GetCountryWithMostHolidays();

            Assert.That(result, Is.TypeOf<ActionResult<string>>());
        }

        [Test]
        public void GetMonthWithMostHolidays_WhenCalled_ReturnString()
        {
            var result = _holidayOptimizerController.GetMonthWithMostHolidays();

            Assert.That(result, Is.TypeOf<ActionResult<string>>());
        }

        [Test]
        public void GetCountryWithMostUniqueHolidays_WhenCalled_ReturnString()
        {
            var result = _holidayOptimizerController.GetCountryWithMostUniqueHolidays();

            Assert.That(result, Is.TypeOf<ActionResult<string>>());
        }

        [Test]
        public void GetLongestSequenceOfHolidays_WhenCalled_ReturnString()
        {
            var result = _holidayOptimizerController.GetLongestSequenceOfHolidays();

            Assert.That(result, Is.TypeOf<ActionResult<string>>());
        }
    }
}