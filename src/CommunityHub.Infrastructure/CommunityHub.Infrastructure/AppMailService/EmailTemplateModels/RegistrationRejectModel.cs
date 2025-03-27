using CommunityHub.Infrastructure.EmailSenderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.AppMailService
{
    public class RegistrationRejectModel : TemplateModelBase
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string RegistrationDate { get; set; }
        public string RejectionReason { get; set; }
    }
}
