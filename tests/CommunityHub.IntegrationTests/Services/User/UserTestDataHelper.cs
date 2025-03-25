using CommunityHub.Infrastructure.Models;
using System.Collections.Generic;

namespace CommunityHub.Tests.Data
{
    public static class UserTestsDataHelper
    {
        public static List<UserInfo> GetUsers()
        {
            return new List<UserInfo>
            {
                new UserInfo
                {
                    Id = 1,
                    FullName = "John Doe",
                    Email = "john@example.com",
                    CountryCode = "+1",
                    ContactNumber = "123456789",
                    Gender = "Male",
                    Location = "City A",
                    MaritalStatus = "Married",
                    HomeTown = "Town A",
                    HouseName = "House A",
                    ApplicationUserId = "app-user-1",
                    SpouseInfo = new SpouseInfo
                    {
                        FullName = "Jane Doe",
                        Email = "jane@example.com",
                        CountryCode = "+1",
                        ContactNumber = "987654321",
                        Gender = "Female",
                        Location = "City A",
                        HomeTown = "Town B"
                    },
                    Children = new List<Children>
                    {
                        new Children { Name = "Child One" },
                        new Children { Name = "Child Two" }
                    }
                },
                new UserInfo
                {
                    Id = 2,
                    FullName = "Alice Smith",
                    Email = "alice@example.com",
                    CountryCode = "+44",
                    ContactNumber = "987654321",
                    Gender = "Female",
                    Location = "City B",
                    MaritalStatus = "Single",
                    HomeTown = "Town C",
                    HouseName = "House B",
                    ApplicationUserId = "app-user-2",
                    SpouseInfo = null,
                    Children = new List<Children>()
                }
            };
        }
    }
}
