using System;

namespace DAL.Entities
{
    public class Event
    {
        public int Id { get; set; }

        /// <summary>
        /// Used only to make sure user gets one next event per topic at a time
        /// </summary>
        public string TopicIdentifier { get; set; }

        public string NextText { get; set; }

        public string NowText { get; set; }

        public DateTime Time { get; set; }

        public int DurationMinutes { get; set; }
    }
}
