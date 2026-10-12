namespace RaceDay.API.Models
{
    public class Result
    {
        public int ResultId { get; set; }

        public int EnrolmentId { get; set; }

        public TimeSpan FinishTime { get; set; }

        public int Position { get; set; }

        public string ResultStatus { get; set; } = string.Empty;

        public DateTime CapturedAt { get; set; }

        public Enrolment? Enrolment { get; set; }
    }
}