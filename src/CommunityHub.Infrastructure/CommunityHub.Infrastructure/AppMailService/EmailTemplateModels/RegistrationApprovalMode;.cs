using CommunityHub.Infrastructure.EmailSenderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.AppMailService
{
    public class RegistrationApprovalModel : TemplateModelBase
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordSetLink { get; set; }
    }
}
