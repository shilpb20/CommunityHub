using System.Data.Common;

namespace CommunityHub.Infrastructure.EmailSenderService
{
    public class RegistrationModel : TemplateModelBase
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Location { get; set; }
        public string UserName { get; set; }
    }
}
