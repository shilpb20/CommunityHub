using CommunityHub.Core.Constants;
using CommunityHub.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.EmailSenderService
{
    public class RegistrationRequestTemplateModel
    {
        public string Link { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Location { get; set; }
        public string UserName { get; set; }

        public RegistrationRequestTemplateModel(
            string baseUrl,
            DateTime registrationDate, 
            string location, 
            string userName)
        {
            Link = HttpHelper.BuildUri(_baseUrl, ApiRoute.Registration.GetAllRequests, queryParameters);
            RegistrationDate = registrationDate;
            Location = location;
            UserName = userName;
        }
    }
}
