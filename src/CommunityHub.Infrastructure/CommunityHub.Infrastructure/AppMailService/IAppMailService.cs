using AppComponents.Email.Models;
using CommunityHub.Infrastructure.AppMailService;
using CommunityHub.Infrastructure.EmailSenderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.EmailService
{
    public interface IAppMailService
    {
        public Task<EmailStatus> SendRegistrationNotificationAsync(RegistrationModel model);
        public Task<EmailStatus> SendRegistrationRequestRejectionNotificationAsync(RegistrationRejectModel model);
        public Task<EmailStatus> SendRegistrationRequestApprovalNotificationAsync(RegistrationApprovalModel model);
    }
}
