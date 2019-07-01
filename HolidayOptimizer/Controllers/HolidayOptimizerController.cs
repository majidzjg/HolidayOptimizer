using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace HolidayOptimizer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HolidayOptimizerController : ControllerBase
    {
        [HttpGet("/countrywithmostholidays")]
        public ActionResult<string> GetCountryWithMostHolidays()
        {
            try
            {
                var publicHolidayList = LoadJson();
                if (publicHolidayList is null)
                {
                    return NotFound();
                }

                var country = publicHolidayList
                    .GroupBy(model => model.CountryCode)
                    .OrderByDescending(model => model.Count())
                    .Select(model => model.FirstOrDefault());

                var countryCode = country.FirstOrDefault().CountryCode;
                var holidayCount = country.Count();

                var result = $"{countryCode} has the most holidays with {holidayCount} days.";

                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("/monthwithmostholidays")]
        public ActionResult<string> GetMonthWithMostHolidays()
        {
            try
            {
                var publicHolidayList = LoadJson();

                var month = publicHolidayList
                    .GroupBy(model => model.Date.Month)
                    .OrderByDescending(model => model.Count())
                    .Select(model => model.FirstOrDefault());

                var monthName = month.FirstOrDefault().Date.ToString("MMMM");
                var holidayCount = month.Count();

                var result = $"{monthName} has the most holidays with {holidayCount} days.";

                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("/countrywithmostuniqueholidays")]
        public ActionResult<string> GetCountryWithMostUniqueHolidays()
        {
            try
            {
                var publicHolidayList = LoadJson();

                var country = publicHolidayList
                    .GroupBy(model => model.Date.DayOfYear)
                    .Where(model => model.Count() == 1)
                    .SelectMany(model => model.ToList())
                    .GroupBy(grp => grp.CountryCode)
                    .OrderByDescending(model => model.Count())
                    .Select(model => model.FirstOrDefault());

                var countryCode = country.FirstOrDefault().CountryCode;
                var holidayCount = country.Count();

                var result = $"{countryCode} has the most unique holidays with {holidayCount} days.";

                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("/longestsequenceofholidays")]
        public ActionResult<string> GetLongestSequenceOfHolidays()
        {
            try
            {
                var publicHolidayList = LoadJson();

                int maximumSequence = 0;
                int sequence = 0;
                DateTime finishDate = new DateTime(2019, 1, 1);

                for (DateTime date = new DateTime(2019, 1, 1); date.Year <= 2020; date = date.AddDays(1))
                {
                    if (publicHolidayList.Any(model => model.Date.DayOfYear == date.DayOfYear))
                    {
                        sequence++;
                    }
                    else
                    {
                        if (sequence > maximumSequence)
                        {
                            maximumSequence = sequence;
                            finishDate = date;
                        }
                        sequence = 0;
                    }
                }

                var result = $"Longest lasting sequence of holidays is {maximumSequence} days which start from {finishDate.AddDays(-maximumSequence).ToString("yyyy-MM-dd")} and finish in {finishDate.ToString("yyyy-MM-dd")}";

                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [SwaggerIgnore]
        public void SaveJson()
        {
            var countryList = new List<string>() {"AD", "AR", "AT", "AU", "AX", "BB", "BE",
                    "BG", "BO", "BR", "BS", "BW", "BY",
                    "BZ", "CA", "CH", "CL", "CN", "CO",
                    "CR", "CU", "CY", "CZ", "DE", "DK",
                    "DO", "EC", "EE", "EG", "ES", "FI",
                    "FO", "FR", "GA", "GB", "GD", "GL",
                    "GR", "GT", "GY", "HN", "HR", "HT",
                    "HU", "IE", "IM", "IS", "IT", "JE",
                    "JM", "LI", "LS", "LT", "LU", "LV",
                    "MA", "MC", "MD", "MG", "MK", "MT",
                    "MX", "MZ", "NA", "NI", "NL", "NO",
                    "NZ", "PA", "PE", "PL", "PR", "PT",
                    "PY", "RO", "RS", "RU", "SE", "SI",
                    "SJ", "SK", "SM", "SR", "SV", "TN",
                    "TR", "UA", "US", "UY", "VA", "VE", "ZA"};

            var clientRequest = new HttpClient();

            clientRequest.BaseAddress = new Uri("https://date.nager.at/api/v2/PublicHolidays/2019/");
            clientRequest.DefaultRequestHeaders.Accept.Clear();

            var publicHolidayList = new List<PublicHolidayModel>();

            foreach (var item in countryList)
            {
                var response = clientRequest.GetAsync(item).Result;

                publicHolidayList.AddRange(response.Content.ReadAsAsync<List<PublicHolidayModel>>().Result);
            }

            System.IO.File.WriteAllText(@"nagerdate.json", JsonConvert.SerializeObject(publicHolidayList.ToArray()));
        }

        [SwaggerIgnore]
        public List<PublicHolidayModel> LoadJson()
        {
            if (System.IO.File.Exists("nagerdate.json"))
            {
                StreamReader streamReader = new StreamReader("nagerdate.json");
                string file = streamReader.ReadToEnd();
                List<PublicHolidayModel> publicHolidayList = JsonConvert.DeserializeObject<List<PublicHolidayModel>>(file);

                return publicHolidayList;
            }
            else
            {
                return null;
            }
        }
    }
}
