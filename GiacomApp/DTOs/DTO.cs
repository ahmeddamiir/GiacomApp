namespace Call_Detail_Record_Business_Intelligence.DTOs
{
    public class DTO
    {
        public long CallerId { get; set; }
        public required long Recipient { get; set; }
        public required DateOnly CallDate { get; set; }
        public required TimeOnly EndTime { get; set; }
        public required short Duration { get; set; }
        public required float Cost { get; set; }
        public required string Reference { get; set; }
        public required string Currency { get; set; }
    }
}
