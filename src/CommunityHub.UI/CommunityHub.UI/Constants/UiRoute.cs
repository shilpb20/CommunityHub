namespace CommunityHub.UI.Constants
{
    public static class UiRoute
    {
        public static class Account
        {
            public const string Register = "ui/register/user-request";
        }

        public static class Admin
        {
            public const string Index = "ui/admin/index";
            public const string RegistrationRequest = "ui/admin/user-requests";

            public const string ApproveRequest = "ui/admin/user-requests/accept";
            public const string RejectRequest = "ui/admin/user-requests/reject";
        }


        public static class Home
        {
            public const string Index = "ui/home/index";
            public const string Privacy = "ui/home/privacy";
        }
    }
}
