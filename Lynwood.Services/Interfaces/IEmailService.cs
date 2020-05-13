using Sabio.Models.Requests;
using System;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IEmailService
    {
        Task Send(EmailSendRequest model);
        Task RegisterEmail(string recipient, Guid token);
    }
}
