using AppComponents.Email.Models;
using AppComponents.Email.Services;
using AppComponents.TemplateEngine;
using Microsoft.Extensions.Logging;

namespace CommunityHub.Infrastructure.EmailService
{
    public class RegistrationEmailSenderService
    {
        private readonly ILogger<RegistrationEmailSenderService> _logger;
        private readonly IModelTemplateEngine _templateEngine;
        private readonly IEmailService _emailService;

        public RegistrationEmailSenderService(
            ILogger<RegistrationEmailSenderService> logger,
            IModelTemplateEngine templateEngine,
            IEmailService emailService)
        {
            _logger = logger;
            _templateEngine = templateEngine;
            _emailService = emailService;
        }

        public void SendRegistrationNotification()
        {
            string template = "EmailTemplates/registration-request.html";

            var emailRequest = new EmailRequest
            {
                To = new List<string> { "shilbh20@gmail.com" },
                Subject = "Test Email from Community Hub",
                HtmlContent = "<h1>This is a test email</h1>"
            };
        }

        //        var headerTemplate = File.ReadAllText("EmailTemplates/_header.html");
        //        var footerTemplate = File.ReadAllText("EmailTemplates/_footer.html");
        //        var emailTemplate = File.ReadAllText("EmailTemplates/new_registration_request.html");

        //        // Replace placeholders with actual content
        //        emailTemplate = emailTemplate.Replace("{{header}}", headerTemplate);
        //emailTemplate = emailTemplate.Replace("{{footer}}", footerTemplate);

        //// Now you can send this combined email content
        //SendEmail(emailTemplate);

        //    }
    }
