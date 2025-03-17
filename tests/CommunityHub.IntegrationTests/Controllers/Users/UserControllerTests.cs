using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.IntegrationTests.Controllers.Users
{
    public class UserControllerTests : BaseTestEnv
    {
        public UserControllerTests(ApplicationStartup application) : base(application)
        {
            _url = "api/users";
        }
    }
}
