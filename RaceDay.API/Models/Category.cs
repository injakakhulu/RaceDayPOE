namespace RaceDay.API.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public int EventId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CategoryType { get; set; } = string.Empty;

        public decimal? Distance { get; set; }

        public int? MinimumAge { get; set; }

        public int? MaximumAge { get; set; }
    }
}