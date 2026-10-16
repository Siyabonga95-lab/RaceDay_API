using System.Net;
using Xunit;

namespace RaceDay_API.Tests
{
    public class AuthTests : IClassFixture<RaceDayWebApplicationFactory>
    {
        private readonly RaceDayWebApplicationFactory _factory;

        public AuthTests(RaceDayWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task Register_AsParticipant_Returns200()
        {
            var client = _factory.CreateClient();
            var response = await TestHelpers.RegisterUser(client, "reg_p1", "Participant");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_AsOrganiser_Returns200()
        {
            var client = _factory.CreateClient();
            var response = await TestHelpers.RegisterUser(client, "reg_o1", "Organiser");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_WithDuplicateUsername_Returns409Conflict()
        {
            var client = _factory.CreateClient();
            await TestHelpers.RegisterUser(client, "dup_user");
            var second = await TestHelpers.RegisterUser(client, "dup_user");
            Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        }

        [Fact]
        public async Task Register_WithInvalidRole_Returns400()
        {
            var client = _factory.CreateClient();
            var response = await TestHelpers.RegisterUser(client, "bad_role", "SuperAdmin");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithValidCredentials_Returns200()
        {
            var client = _factory.CreateClient();
            await TestHelpers.RegisterUser(client, "login_p1", "Participant");
            var response = await TestHelpers.LoginUser(client, "login_p1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithWrongPassword_Returns401()
        {
            var client = _factory.CreateClient();
            await TestHelpers.RegisterUser(client, "wrongpw", "Participant");
            var response = await TestHelpers.LoginUser(client, "wrongpw", "WrongPassword!");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithNonExistentUser_Returns401()
        {
            var client = _factory.CreateClient();
            var response = await TestHelpers.LoginUser(client, "ghost_user");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}