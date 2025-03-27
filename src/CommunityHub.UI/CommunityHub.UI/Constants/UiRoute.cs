namespace CommunityHub.UI.Constants
{
    public static class UiRoute
    {
        public static class Account
        {
            public const string Register = "/register/user-request";
        }

        public static class Admin
        {
            public const string Index = "/admin/index";
            public const string RegistrationRequest = "/admin/user-requests";

            public const string ApproveRequest = "/admin/user-requests/accept";
            public const string RejectRequest = "/admin/user-requests/reject";
        }


        public static class Home
        {
            public const string Index = "/home/index";
            public const string Privacy = "/home/privacy";
        }
    }
}
