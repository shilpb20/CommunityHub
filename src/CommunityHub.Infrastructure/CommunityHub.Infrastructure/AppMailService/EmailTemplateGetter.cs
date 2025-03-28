using System;
using System.IO;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.AppMailService
{
    public static class EmailTemplateGetter
    {
        public static async Task<string> GetTemplateAsync(string dirPath, string templateFileName)
        {
            try
            {
                var headerTemplate = await File.ReadAllTextAsync(Path.Combine(dirPath, TemplateNames.Header));
                var footerTemplate = await File.ReadAllTextAsync(Path.Combine(dirPath, TemplateNames.Footer));
                var emailTemplate = await File.ReadAllTextAsync(Path.Combine(dirPath, templateFileName));

                // Convert logo image to Base64
                string logoPath = Path.Combine(dirPath, "_logo.png");
                if (File.Exists(logoPath))
                {
                    string base64Image = Convert.ToBase64String(await File.ReadAllBytesAsync(logoPath));
                    emailTemplate = emailTemplate.Replace("{{logo}}", base64Image);
                }
                else
                {
                    throw new FileNotFoundException("Logo image not found.", logoPath);
                }

                emailTemplate = emailTemplate.Replace("{{header}}", headerTemplate);
                emailTemplate = emailTemplate.Replace("{{footer}}", footerTemplate);

                return emailTemplate;
            }
            catch (FileNotFoundException ex)
            {
                throw new InvalidOperationException("One or more template files were not found.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while processing the email template.", ex);
            }
        }
    }
}
