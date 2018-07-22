using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalaryPeeker.API.Models;

namespace SalaryPeeker.API.Persistence.Repository
{
    public class SalaryPeeperRepository : ISalaryPeeperRepository
    {
        private readonly SalaryPeekerDbContext _context;

        public SalaryPeeperRepository(SalaryPeekerDbContext context)
        {
            _context = context;
        }

        public Task<List<SalaryRecord>> GetAllSalaryRecords()
        {
            return _context.SalaryRecords.ToListAsync();
        }

        public Task<List<SalaryRecord>> GetFilteredSalaryRecords(string agency, int? lowerLimit, int? upperLimit,
            int? year)
        {
            //
            // Salary range will always come in a pair, so just check lower limit
            var useSalaryRange = lowerLimit != null;
            var useYear = year != null;
            var useAgency = agency != "All";
            
            var filteredSalaryRecords = _context.SalaryRecords.AsQueryable();
            
            if (useAgency)
            {
                var textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
                agency = textInfo.ToTitleCase(agency);

                filteredSalaryRecords = from m in filteredSalaryRecords
                    where m.Agency == agency
                    select m;
            }

            if (useSalaryRange)
                filteredSalaryRecords = from m in filteredSalaryRecords
                    where m.TotalPayBenefits >= lowerLimit && m.TotalPayBenefits < upperLimit
                    select m;

            if (useYear)
                filteredSalaryRecords = from m in filteredSalaryRecords
                    where m.Year == year
                    select m;

            return filteredSalaryRecords.ToListAsync();
        }

        public Task<List<SalaryRecord>> GetEmployeeSalaryRecords(string employeeName)
        {
            return _context.SalaryRecords.Where(m => m.EmployeeName.ToLower().Contains(employeeName.ToLower())).ToListAsync();
        }

        public Task<List<SalaryRecord>> GetJobSalaryRecords(string title)
        {
            return _context.SalaryRecords.Where(m => m.JobTitle.ToLower().Contains(title.ToLower())).ToListAsync();
        }
    }
}