using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using RaceDay_API.Models;

namespace RaceDay_API.Tests
{
    public static class TestHelpers
    {
        public static async Task<HttpResponseMessage> RegisterUser(
            HttpClient client, string username, string role = "Participant")
        {
            var dto = new RaceDay_API.DTOs.RegisterDTO
            {
                Username = username,
                Password = "Password123!",
                Role = role,
                Name = "Test User",
                Email = $"{username}@test.com",
                Phone = "0825550000"
            };
            return await client.PostAsJsonAsync("/api/auth/register", dto);
        }

        public static async Task<HttpResponseMessage> LoginUser(
            HttpClient client, string username, string password = "Password123!")
        {
            var dto = new RaceDay_API.DTOs.LoginDto { Username = username, Password = password };
            return await client.PostAsJsonAsync("/api/auth/login", dto);
        }

        public static async Task<HttpClient> CreateLoggedInClient(
            RaceDayWebApplicationFactory factory, string username, string role)
        {
            var client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true
            });

            var reg = await RegisterUser(client, username, role);
            if (!reg.IsSuccessStatusCode)
            {
                var body = await reg.Content.ReadAsStringAsync();
                throw new Exception($"Registration failed ({(int)reg.StatusCode}). Body: {body}");
            }

            var login = await LoginUser(client, username);
            if (!login.IsSuccessStatusCode)
            {
                var body = await login.Content.ReadAsStringAsync();
                throw new Exception($"Login failed ({(int)login.StatusCode}). Body: {body}");
            }

            return client;
        }
    }
}