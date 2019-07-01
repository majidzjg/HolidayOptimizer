using System;

namespace HolidayOptimizer
{
    public class PublicHolidayModel
    {
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string CountryCode { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public bool Fixed { get; set; }
        public bool Global { get; set; }
        public int? LaunchYear { get; set; }
    }
}
