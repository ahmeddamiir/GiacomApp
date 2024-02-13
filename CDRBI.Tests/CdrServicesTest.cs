using Call_Detail_Record_Business_Intelligence.Data;
using Call_Detail_Record_Business_Intelligence.Services;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CDRBI.Tests
{
    public class CdrServicesTest
    {
        private readonly ILogger _logger;
        public CdrServicesTest(ILogger logger)
        {
            _logger = logger;
        }
        [Fact]
        public async void GetAverageCallCost_WhenDatabaseContainsValidCostRecords_CalculateAverageCost()
        {
            // Arrange
            var mockDbContext = new Mock<ApplicationDbContext>();

            var cdrData = new List<Cdr>
            {
                new Cdr {cost = 1, recipient = 124325, currency = "GBP", duration = 213, call_date = null, end_time = null, reference = null},
                new Cdr {cost = 2, recipient = 124325, currency = "GBP", duration = 213, call_date = null, end_time = null, reference = null},
                new Cdr {cost = 3, recipient = 124325, currency = "GBP", duration = 213, call_date = null, end_time = null, reference = null},
            };

            var mockDbSet = new Mock<DbSet<Cdr>>();
            mockDbSet.As<IQueryable<Cdr>>().Setup(m => m.Provider).Returns(cdrData.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Cdr>>().Setup(m => m.Expression).Returns(cdrData.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Cdr>>().Setup(m => m.ElementType).Returns(cdrData.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Cdr>>().Setup(m => m.GetEnumerator()).Returns(cdrData.AsQueryable().GetEnumerator());
            mockDbContext.Setup(c => c.Cdrs).Returns(mockDbSet.Object); 

            var sut = new CdrServices(logger: null, mockDbContext.Object);
            // Act
            var result = await sut.GetAverageCallCost();
            //Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(10, okResult.Value);
        }
    }
}