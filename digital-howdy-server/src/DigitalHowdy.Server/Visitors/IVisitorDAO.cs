using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalHowdy.Server.Visitors
{
    public interface IVisitorDAO
    {
        Task<IEnumerable<VisitorDTO>> GetAllVisitors(string query = "");
        Task<VisitorDTO> GetVisitorByPhone(string phone);
        Task<VisitorDTO> InsertVisitor(VisitorInsertDTO insertVisitor);
    }
}