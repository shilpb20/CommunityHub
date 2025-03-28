using AppComponents.Email.Models;
using AppComponents.Email.Services;
using AppComponents.TemplateEngine;
using CommunityHub.Infrastructure.AppMailService;
using CommunityHub.Infrastructure.AppMailService.EmailConstants;
using CommunityHub.Infrastructure.EmailSenderService;
using CommunityHub.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityHub.Infrastructure.Settings;

namespace CommunityHub.Infrastructure.EmailService
{
    public class AppMailService : IAppMailService
    {
        private readonly string _baseUrl;
        private readonly ILogger<AppMailService> _logger;
        private readonly IModelTemplateEngine _templateEngine;
        private readonly IEmailService _emailService;

        private readonly EmailAppSettings _settings;

        public AppMailService(
            ILogger<AppMailService> logger,
            IModelTemplateEngine templateEngine,
            IEmailService emailService,
            AppSettings appSettings)
        {
            _logger = logger;

            _templateEngine = templateEngine;
            _emailService = emailService;

            _baseUrl = appSettings.BaseUrl;
            _settings = appSettings.EmailAppSettings;
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
            string passwordSetLink = $"{_baseUrl}/set-password?email={model.Email}&token={model.PasswordSetLink}";
            model.PasswordSetLink = passwordSetLink;

            model.Title = EmailSubject.RegistrationRequestApproval;

            string template = await EmailTemplateGetter.GetTemplateAsync(_settings.EmailTemplateDirectory, TemplateNames.RegistrationRequestApproval);
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
