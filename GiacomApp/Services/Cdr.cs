using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace Call_Detail_Record_Business_Intelligence.Services
{
    public class Cdr
    {
        public long? caller_id { get; set; }
        public required long? recipient { get; set; }
        public required DateOnly? call_date { get; set; }
        public required TimeSpan? end_time { get; set; }
        public required short? duration { get; set; }
        public required double? cost { get; set; }
        public required string? reference { get; set; }
        public required string? currency { get; set; }
    }
}
