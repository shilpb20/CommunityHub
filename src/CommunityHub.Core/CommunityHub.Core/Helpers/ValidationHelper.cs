using CommunityHub.Core.Messages;

namespace CommunityHub.Core.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateNullOrEmptyString(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(String.Format(ErrorMessages.ValueNullOrEmpty, paramName));
            }
        }
    }
}
