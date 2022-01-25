using System.Threading.Tasks;
using DAL.DTOs;

namespace API.Interfaces
{
    public interface IMailService
    {
        void SendEmailAsync(EmailDto emailDto);
    }
}