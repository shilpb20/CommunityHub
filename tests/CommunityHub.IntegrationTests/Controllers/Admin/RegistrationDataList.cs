using CommunityHub.Core.Dtos;
using CommunityHub.Core.Dtos.RegistrationData;

namespace CommunityHub.IntegrationTests.Controllers.Admin
{
    public static class RegistrationDataList
    {
        public static List<RegistrationDataCreateDto> GetRequests()
        {
            return new List<RegistrationDataCreateDto>()
            {
                new RegistrationDataCreateDto()
                {
                    UserDetails = new UserDetailsCreateDto()
                    {
                        FullName = "Alice Springs",
                        Email = "alice.springs@gmail.com",
                        CountryCode = "+61",
                        ContactNumber = "0123456789",
                        Gender = "Female",
                        MaritalStatus = "Unmarried",
                        Location = "Northern Territory",
                        HomeTown = "Atlanta",
                        HouseName = "Naomi home"
                    }
                },
                new RegistrationDataCreateDto()
                {
                    UserDetails = new UserDetailsCreateDto()
                    {
                        FullName = "Alex Springs",
                        Email = "alex.springs@gmail.com",
                        CountryCode = "+61",
                        ContactNumber = "0123456790",
                        Gender = "Male",
                        MaritalStatus = "Married",
                        Location = "Southern Australia",
                        HomeTown = "Atlanta",
                        HouseName = "Naomi home"
                    },
                    SpouseDetails = new SpouseDetailsCreateDto()
                    {
                        FullName = "Alicia Adams",
                        Email = "alicia.adams@gmail.com",
                        CountryCode = "+61",
                        ContactNumber = "0123123412",
                        HomeTown = "Georgia",
                        HouseName = "Adams Family",
                    },
                    Children = new List<string>() { "Anna", "Ben", "Cairo" }
                },
                new RegistrationDataCreateDto()
                {
                    UserDetails = new UserDetailsCreateDto()
                    {
                        FullName = "Alice Smith",
                        Email = "alice.smith@gmail.com",
                        CountryCode = "+61",
                        ContactNumber = "0123456756",
                        Gender = "Female",
                        MaritalStatus = "Unmarried",
                        Location = "New South Whales",
                        HomeTown = "Sydney",
                        HouseName = "Smith Family"
                    },
                    Children = new List<string>() { "Ari", "Garry", "Cherry", "Fairy" }
                },
                new RegistrationDataCreateDto()
                {
                    UserDetails = new UserDetailsCreateDto()
                    {
                        FullName = "Daisy Shaw",
                        Email = "daisy.shaw@gmail.com",
                        CountryCode = "+61",
                        ContactNumber = "0123456742",
                        Gender = "Female",
                        MaritalStatus = "Widow",
                        Location = "New South Whales",
                        HomeTown = "Blue Mountains",
                        HouseName = "Shaw Clan"
                    },
                }
            };
        }
    }
}
