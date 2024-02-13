using Call_Detail_Record_Business_Intelligence.Data;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Call_Detail_Record_Business_Intelligence.Services
{
    public class CdrServices : ICdrServices
    {
        private readonly ILogger<CdrServices> _logger;
        private readonly ApplicationDbContext _dbContext;
        
        public CdrServices(ILogger<CdrServices> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        public async Task<IActionResult> GetAverageCallCost()
        {
            try
            {
                var averageCost = await _dbContext.Cdrs.AverageAsync(cdr => cdr.cost);
                return new OkObjectResult(averageCost);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new
                {
                    errorMessage = ex.Message, errorType = ex.GetType().Name
                });
            }
        }
        public async Task<IActionResult> GetLongestCall()
        {
            try
            {
                var longestCall = await _dbContext.Cdrs.MaxAsync(cdr => cdr.duration);
                return new OkObjectResult(longestCall);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new
                {
                    errorMessage = ex.Message, errorType = ex.GetType().Name
                });
            }
        }
        public async Task<IActionResult> GetAverageCallsPerSpecificTime(DateOnly startDate, DateOnly endDate)
        {
            try
            {
                var callsInPeriod = _dbContext.Cdrs.Where(cdr =>
                    cdr.call_date >= startDate && cdr.call_date <= endDate);

                var totalDays =
                    (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).TotalDays;

                var averageCallsPerDay = totalDays == 0 ? 0 : await callsInPeriod.CountAsync() / totalDays;
                _logger.LogInformation($"startDate: {startDate}, endDate: {endDate}");

                return new OkObjectResult(averageCallsPerDay);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new
                {
                    errorMessage = ex.Message, errorType = ex.GetType().Name
                });
            }
        }
    }
}
