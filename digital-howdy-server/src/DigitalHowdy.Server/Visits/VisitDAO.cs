using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using System.Threading.Tasks;
using DbReader;
using DigitalHowdy.Server.Database;
using DigitalHowdy.Server.Employees;

namespace DigitalHowdy.Server.Visits
{
    public class VisitDAO : IVisitDAO
    {
        private IDbConnection _dbConnection;
        private ISqlQueries _sqlQueries;
        private readonly Random _random = new Random();

        public VisitDAO(IDbConnection dbConnection, ISqlQueries sqlQueries)
        {
            _dbConnection = dbConnection;
            _sqlQueries = sqlQueries;
        }

        public async Task<IEnumerable<VisitDTO>> GetAllVisits(string query = "", bool exact = false, bool currentlyIn = false)
        {
            IEnumerable<VisitDTO> results;
            if (query == "" || query == null)
            {
                var queryString = currentlyIn ? _sqlQueries.GetAllCurrentVisits : _sqlQueries.GetAllVisits;
                results = await _dbConnection.ReadAsync<VisitDTO>(queryString);
            }
            else
            {
                query = exact ? query : $"%{query}%";
                var queryString = currentlyIn ? _sqlQueries.GetAllCurrentVisitsByQuery : _sqlQueries.GetAllVisitsByQuery;

                results = await _dbConnection.ReadAsync<VisitDTO>(queryString, new { query = query });
            }

            return results;
        }

        public async Task<VisitDTO> GetVisitById(long id)
        {
            var results = await _dbConnection.ReadAsync<VisitDTO>(_sqlQueries.GetVisitById, new { id = id });

            return results.FirstOrDefault();
        }

        public async Task<VisitDTO> InsertVisit(VisitInsertDTO newVisit)
        {
            long reference = _random.Next(10000, 99999);

            var compare = await _dbConnection.ReadAsync<VisitDTO>(_sqlQueries.RemoveVisitReference, new { reference = reference });

            try
            {
                var visit = new VisitInsertDTO
                {
                    VisitorId = newVisit.VisitorId,
                    EmployeeId = newVisit.EmployeeId,
                    StartDate = newVisit.StartDate,
                    Reference = reference
                };

                await _dbConnection.ExecuteAsync(_sqlQueries.InsertVisit, visit);
                var visitId = await _dbConnection.ExecuteScalarAsync<long>(_sqlQueries.GetLastInsertId);

                var createdVisit = await GetVisitById(visitId);

                return createdVisit;
            }
            catch (Exception e)
            {
                throw new EmployeeNotFoundException("Employee not found!" + e);
            }
        }

        public async Task<bool> UpdateEndDateVisit(VisitUpdateDTO newUpdate)
        {
            var affectedRows = await _dbConnection.ExecuteAsync(_sqlQueries.UpdateEndDateVisit, new { id = newUpdate.Id, EndDate = DateTime.Now });

            return affectedRows != 0;
        }

        public async Task<bool> DeleteVisit(long id)
        {
            var affectedRows = await _dbConnection.ExecuteAsync(_sqlQueries.DeleteVisit, new { id = id });

            return affectedRows == 1;
        }

        public async Task<bool> RemoveVisitReference(long reference)
        {
            var affectedRows = await _dbConnection.ExecuteAsync(_sqlQueries.RemoveVisitReference, new { reference = reference });

            return affectedRows == 1;
        }
    }
}