using System.Net;
using System.Net.Http.Json;
using RaceDay_API.Models;
using Xunit;

namespace RaceDay_API.Tests
{
    public class RoleAuthorizationTests : IClassFixture<RaceDayWebApplicationFactory>
    {
        private readonly RaceDayWebApplicationFactory _factory;

        public RoleAuthorizationTests(RaceDayWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task CreateEvent_AsParticipant_Returns403()
        {
            var client = await TestHelpers.CreateLoggedInClient(_factory, "role_p1", "Participant");

            var newEvent = new Event
            {
                EventName = "Test Run",
                EventDescription = "Desc",
                EventDate = DateTime.Now.AddDays(7),
                EventLocation = "Cape Town",
                EventDistance = "10KM",
                EventType = "Running"
            };

            var response = await client.PostAsJsonAsync("/api/events", newEvent);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task CreateEvent_Unauthenticated_Returns403()
        {
            var client = _factory.CreateClient();

            var newEvent = new Event
            {
                EventName = "Anonymous Event",
                EventDescription = "Desc",
                EventDate = DateTime.Now.AddDays(7),
                EventLocation = "Durban",
                EventDistance = "5KM",
                EventType = "Walking"
            };

            var response = await client.PostAsJsonAsync("/api/events", newEvent);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task EnrollInEvent_AsOrganiser_Returns403()
        {
            var client = await TestHelpers.CreateLoggedInClient(_factory, "role_o1", "Organiser");

            var enrollment = new EventEnrollment { EventID = 1, CategoryID = 1 };
            var response = await client.PostAsJsonAsync("/api/enrollments", enrollment);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task CaptureResult_AsParticipant_Returns403()
        {
            var client = await TestHelpers.CreateLoggedInClient(_factory, "capture_p1", "Participant");

            var result = new Result
            {
                EnrollmentID = 1,
                Result_finish_time = TimeSpan.FromMinutes(30),
                Participant_position = "1"
            };

            var response = await client.PostAsJsonAsync("/api/results", result);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}