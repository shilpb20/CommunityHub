using CommunityHub.Core.Constants;
using CommunityHub.Core.Models;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace CommunityHub.UI.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(HttpClient httpClient, IHttpRequestSender requestSender, IOptions<AppSettings> options) : base(httpClient, requestSender, options) { }


        public async Task<ApiResponse<bool>> SetNewPasswordAsync(SetPassword model)
        {
            var result = await _requestSender.SendPostRequestAsync<SetPassword, bool>(_httpClient, ApiRoute.Account.SetPassword, model);
            return result;
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(Login model)
        {
            var result = await _requestSender.SendPostRequestAsync<Login, LoginResponse>(_httpClient, ApiRoute.Account.Login, model);
            return result;
        }

        public async Task<ApiResponse<bool>> SendPasswordResetEmailAsync(string email)
        {
            string baseAddress = _appSettings.AppUrl.TrimEnd('/'); ;
            var url = $"{baseAddress}{UiRoute.Account.ResetPassword}";

            var emailLink = new PasswordLink() { Email = email, Url = url };
            var result = await _requestSender.SendPostRequestAsync<PasswordLink, bool>(_httpClient, ApiRoute.Account.SendPasswordResetEmail, emailLink);
            return result;
        }
    }
}
