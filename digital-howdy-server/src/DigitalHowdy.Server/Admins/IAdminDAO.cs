using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalHowdy.Server.Admins
{
    public interface IAdminDAO
    {
        Task<IEnumerable<AdminDTO>> GetAllAdmins();
        Task<AdminDTO> InsertAdmin(AdminInsertDTO insertAdmin);
        Task<bool> DeleteAdmin(long adminId);
        Task<string> LoginAdmin(AdminLoginDTO adminLogin);
        Task<AdminDTO> RegisterAdmin(AdminRegisterDTO adminRegister);
    }
}