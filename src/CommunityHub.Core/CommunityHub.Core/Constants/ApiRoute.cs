namespace CommunityHub.Core.Constants
{
    public static class ApiRoute
    {
        public static class Users
        {
            /// <summary>Gets all users.</summary>
            public const string GetAll = $"{ApiPrefixes.Users}/all";

            /// <summary>Finds a user by - email or contact or spouse email or spouse contact</summary>
            public const string Find = $"{ApiPrefixes.Users}/find";
        }

        public static class Registration
        {
            /// <summary>Adds or retrieves registration requests</summary>
            public const string Request = $"{ApiPrefixes.Account}/register";

            /// <summary>Gets registration requests with id</summary>
            public const string GetRequestById = $"{ApiPrefixes.Account}/register/{{id:int}}";
        }

        public static class Admin
        {
            /// <summary>Rejects request by id</summary>
            public const string RejectRequestById = $"{ApiPrefixes.Admin}/request/reject/{{id:int}}";

            /// <summary>Gets request by id</summary>
            public const string ApproveRequestById = $"{ApiPrefixes.Admin}/request/approve/{{id:int}}";
        }
    }
}
