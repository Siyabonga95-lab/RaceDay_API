using System.Net;
using System.Net.Http.Json;
using RaceDay_API.Models;
using Xunit;

namespace RaceDay_API.Tests
{
    public class EnrollmentTests : IClassFixture<RaceDayWebApplicationFactory>
    {
        private readonly RaceDayWebApplicationFactory _factory;

        public EnrollmentTests(RaceDayWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task GetMyEnrollments_AsParticipant_Returns200()
        {
            var client = await TestHelpers.CreateLoggedInClient(_factory, "enr_p1", "Participant");
            var response = await client.GetAsync("/api/enrollments/participant");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetMyEnrollments_AsOrganiser_Returns403()
        {
            var client = await TestHelpers.CreateLoggedInClient(_factory, "enr_o1", "Organiser");
            var response = await client.GetAsync("/api/enrollments/participant");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task EnrollInEvent_Unauthenticated_Returns403()
        {
            var client = _factory.CreateClient();
            var enrollment = new EventEnrollment { EventID = 1, CategoryID = 1 };
            var response = await client.PostAsJsonAsync("/api/enrollments", enrollment);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}