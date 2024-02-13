using Microsoft.AspNetCore.Mvc;

namespace Call_Detail_Record_Business_Intelligence.Services
{
    public interface ICdrServices
    {
        Task<IActionResult> GetAverageCallCost();
        Task<IActionResult> GetLongestCall();
        Task<IActionResult> GetAverageCallsPerSpecificTime(DateOnly startDate, DateOnly endDate);
    }
}
