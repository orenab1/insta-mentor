using System;

namespace DAL.DTOs.Full
{
    public class EventDto
    {
        /// <summary>
        /// Used only to make sure user gets one next event per topic at a time
        /// </summary>
        public string TopicIdentifier { get; set; }

        public string NextText { get; set; }

        public string NowText { get; set; }

        public DateTime UtcTime { get; set; }

        public int DurationMinutes { get; set; }
    }
}