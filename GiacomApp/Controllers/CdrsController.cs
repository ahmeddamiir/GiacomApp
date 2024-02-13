using Call_Detail_Record_Business_Intelligence.Data;
using Call_Detail_Record_Business_Intelligence.Services;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Call_Detail_Record_Business_Intelligence.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CdrController : ControllerBase
    {
        private readonly ILogger<CdrController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly ICdrServices _cdrServices;

        public CdrController(ICdrServices cdrServices, ApplicationDbContext dbContext, ILogger<CdrController> logger)
        {
            _logger = logger;
            _cdrServices = cdrServices;
            _dbContext = dbContext;
        }

        /*
         * It's a bad practise to put the business (Logic) of an end point in the controller 
         * And it's better to put it in somewhere else (Like what's in Cdr Services
         */
        [HttpPost]
        [Route("UploadCSV")]
        public async Task<IActionResult> UploadCSV(IFormFile csvFile)
        {
            try
            {
                if (csvFile == null || csvFile.Length == 0)
                {
                    return BadRequest("Wrong file");
                }
                await ProcessCsvData(csvFile);
                return Ok("File Uploaded Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task ProcessCsvData(IFormFile csvFile)
        {
            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            {
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true
                };
                using (var csvReader = new CsvReader(reader, csvConfig))
                {
                    var cdrRecords = csvReader.GetRecords<dynamic>();
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
        [HttpGet]
        [Route("AverageCallCost")]
        public async Task<IActionResult> AverageCallCost()
        {
            return await _cdrServices.GetAverageCallCost();
        }

        [HttpGet]
        [Route("LongestCall")]
        public async Task<IActionResult> LongestCall()
        {
            return await _cdrServices.GetLongestCall();
        }

        [HttpGet]
        [Route("AverageCallsPerSpecificTime")]
        public async Task<IActionResult> AverageCallsPerSpecificTime(DateOnly startDate, DateOnly endDate)
        {
            return await _cdrServices.GetAverageCallsPerSpecificTime(startDate, endDate);
        }

        /* Process Data Dynamically...
        private List<dynamic>? uploadedRecords = null;
        [HttpPost]
        [Route("UploadCSVtest")]
        public async Task<List<dynamic>> UploadCSVtest(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
                return uploadedRecords;

            try
            {
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true
                    };
                    return uploadedRecords = await csvReader.GetRecordsAsync<dynamic>().ToListAsync();
                }

            } 
            catch (Exception ex)
            {
                return uploadedRecords;
            }
        }

        [HttpGet]
        [Route("AverageCallCosttest")]
        public async Task<IActionResult> AverageCallCosttest()
        {
            if (uploadedRecords == null)
                return BadRequest("Upload a CSV file first.");

            try
            {
                var averageCost = uploadedRecords.Average(cdr => cdr.cost);
                return Ok(averageCost);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error calculating average: {ex.Message}");
            }
        }
         */
        /*
    // For large csv files upload in batches
        private async Task ProcessCsvDataBatches(IFormFile csvFile)
        {
            int batchSize = 800;

            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var cdrRecords = csvReader.GetRecords<Cdr>();

                List<Cdr> batch = new List<Cdr>(); 
                foreach (var cdr in cdrRecords)
                {
                    if (!ValidateCdr(cdr))
                    {
                        _logger.LogWarning($"Invalid CDR Record: {cdr}");
                        continue;
                    }

                    batch.Add(cdr);

                    if (batch.Count >= batchSize)
                    {
                        _dbContext.Cdrs.AddRange(batch); 
                        await _dbContext.SaveChangesAsync();
                        batch.Clear(); // Reset the batch for the next chunk
                    }
                }

                // Save any remaining records in the last batch
                if (batch.Count > 0)
                {
                    _dbContext.Cdrs.AddRange(batch);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
        private bool ValidateCdr(Cdr cdr)
        {

            bool isValid = cdr.caller_id >= 0 && cdr.recipient >= 0 &&
                cdr.call_date != null && cdr.duration >= 0;
            if (isValid)
            {
                if (cdr.caller_id <= 0)
                {
                    _logger.LogWarning($"Invalid caller id in the record {cdr}");
                }
                if (cdr.recipient <= 0)
                {
                    _logger.LogWarning($"Invalid recipient in the record {cdr}");
                }
                if (cdr.duration <= 0)
                {
                    _logger.LogWarning($"Invalid duration in the record {cdr}");
                }
            }
            return isValid;
        }
        */
        /*
        This is not the best design as I am supposed to take the Data Transfer Object "DTO" as a parameter, then map it.
        But I wrote it like "receiving the cdr object in my API then insert it in the database" for 2 reasons:-
        1- Simplicity
        2- I'm not that much aware of the correct way of writing it
        */
    }
}
