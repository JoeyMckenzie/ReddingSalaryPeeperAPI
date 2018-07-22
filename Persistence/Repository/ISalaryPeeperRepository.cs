using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalaryPeeker.API.Models;

namespace SalaryPeeker.API.Persistence.Repository
{
    public interface ISalaryPeeperRepository
    {
        Task<List<SalaryRecord>> GetAllSalaryRecords();

        Task<List<SalaryRecord>> GetFilteredSalaryRecords(string agency, int? lowerLimit, int? upperLimit, int? year);

        Task<List<SalaryRecord>> GetEmployeeSalaryRecords(string employeeName);

        Task<List<SalaryRecord>> GetJobSalaryRecords(string title);
    }
}