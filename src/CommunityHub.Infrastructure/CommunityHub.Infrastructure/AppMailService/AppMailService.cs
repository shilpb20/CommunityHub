using AppComponents.Email.Models;
using AppComponents.Email.Services;
using AppComponents.TemplateEngine;
using CommunityHub.Infrastructure.AppMailService;
using CommunityHub.Infrastructure.AppMailService.EmailConstants;
using CommunityHub.Infrastructure.EmailSenderService;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.EmailService
{
    public class AppMailService : IAppMailService
    {
        private readonly ILogger<AppMailService> _logger;
        private readonly IModelTemplateEngine _templateEngine;
        private readonly IEmailService _emailService;

        private readonly EmailAppSettings _settings;
        private readonly string _adminEmail;
        public AppMailService(
            ILogger<AppMailService> logger,
            IModelTemplateEngine templateEngine,
            IEmailService emailService,
            EmailAppSettings emailAppSettings)
        {
            _logger = logger;
            _templateEngine = templateEngine;
            _emailService = emailService;
            _settings = emailAppSettings;
        }

        public async Task<EmailStatus> SendRegistrationNotificationAsync(RegistrationModel model)
        {
            model.Title = EmailSubject.RegistrationNotification;

            string template = await EmailTemplateGetter.GetTemplateAsync(_settings.EmailTemplateDirectory, TemplateNames.RegistrationNotification);
            string content = _templateEngine.Render(template, model);

            var emailRequest = new EmailRequest
            {
                To = new List<string> { _settings.AdminEmail },
                Subject = model.Title,
                HtmlContent = content
            };

            return await _emailService.SendEmailAsync(emailRequest);
        }

        public async Task<EmailStatus> SendRegistrationRequestApprovalNotificationAsync(RegistrationApprovalModel model)
        {
            model.Title = EmailSubject.RegistrationRequestApproval;

            string template = await EmailTemplateGetter.GetTemplateAsync(_settings.EmailTemplateDirectory, TemplateNames.RegistrationRequestApproval);
            string content = _templateEngine.Render(template, model);

            var emailRequest = new EmailRequest
            {
                //TODO: replace user email
                To = new List<string> { _adminEmail },
                Subject = model.Title,
                HtmlContent = content
            };

            return await _emailService.SendEmailAsync(emailRequest);
        }

        public async Task<EmailStatus> SendRegistrationRequestRejectionNotificationAsync(RegistrationRejectModel model)
        {
            model.Title = EmailSubject.RegistrationRequestRejection;

            string template = await EmailTemplateGetter.GetTemplateAsync(_settings.EmailTemplateDirectory, TemplateNames.RegistrationRequestRejection);
            string content = _templateEngine.Render(template, model);

            var emailRequest = new EmailRequest
            {
                //TODO: replace user email
                To = new List<string> { _settings.AdminEmail },
                Subject = model.Title,
                HtmlContent = content
            };

            return await _emailService.SendEmailAsync(emailRequest);
        }
    }
}
