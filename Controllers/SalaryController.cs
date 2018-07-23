using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SalaryPeeker.API.Persistence.Repository;

namespace SalaryPeeker.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controller for retrieving data 
    /// </summary>
    [Route("api/[controller]")]
    [EnableCors("SalaryPeeperPolicy")]
    public class SalaryController : Controller
    {
        private readonly ISalaryPeeperRepository _repository;

        public SalaryController(ISalaryPeeperRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Return all salary data from database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AllSalaryData()
        {
            var model = await _repository.GetAllSalaryRecords();
            
            return Ok(model);
        }

        /// <summary>
        /// Retrieve salaries by agency, range, year, or a combination of all three
        /// 
        /// GET: /api/salary-data/:agency/:lowerLimit/:upperLimit/:year
        /// </summary>
        /// <param name="agency">Shasta County office, or City of Redding office</param>
        /// <param name="lowerLimit">Lower limit to salary</param>
        /// <param name="upperLimit">Upper limit to salary</param>
        /// <param name="year">Salary data fiscal year</param>
        /// <returns>List of salary data with given parameters</returns>
        [HttpGet("{agency}")]
        [HttpGet("{agency}/{lowerLimit}/{upperLimit}")]
        [HttpGet("{agency}/{lowerLimit}/{upperLimit}/{year}")]
        public async Task<IActionResult> GetFilteredSalaries(string agency, int? lowerLimit, int? upperLimit, int? year)
        {
            var model = await _repository.GetFilteredSalaryRecords(agency, lowerLimit, upperLimit, year);

            return Ok(model);
        }

        
        /// <summary>
        /// Retrieve salary by employee name
        ///
        /// GET /api/salary-data/employee/:name
        /// </summary>
        /// <param name="employeeName">Employee name, either first/last, or a combination of both</param>
        /// <returns>Salary by employee name</returns>
        [HttpGet("employee/{employeeName}")]
        public async Task<IActionResult> SearchByEmployeeName(string employeeName)
        {
            var model = await _repository.GetEmployeeSalaryRecords(employeeName);

            return Ok(model);
        }
        
        /// <summary>
        /// Retrive salaries by job title
        ///
        /// GET /api/salary-data/job/:title
        /// </summary>
        /// <param name="title">Job title</param>
        /// <returns>Salaries by job title/descriptor</returns>
        [HttpGet("job/{title}")]
        public async Task<IActionResult> SearchByJobTitle(string title)
        {
            var model = await _repository.GetJobSalaryRecords(title);

            return Ok(model);
        }
        
    }
}