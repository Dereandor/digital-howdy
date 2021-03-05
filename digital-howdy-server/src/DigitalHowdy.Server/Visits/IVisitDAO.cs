using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalHowdy.Server.Visits
{
    public interface IVisitDAO
    {
        Task<IEnumerable<VisitDTO>> GetAllVisits(string query = "", bool exact = false, bool currentlyIn = false);

        Task<VisitDTO> GetVisitById(long id);

        Task<VisitDTO> InsertVisit(VisitInsertDTO newVisit);

        Task<bool> UpdateEndDateVisit(VisitUpdateDTO newUpdate);

        Task<bool> DeleteVisit(long id);
    }
}