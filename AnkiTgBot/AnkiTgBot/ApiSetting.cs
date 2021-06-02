using System.Net.Http;
using System;

namespace AnkiTgBot
{
    public static class ApiSetting
    {
        public static string Base = $"https://localhost:44322/";

        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri("https://localhost:44322/");
        }
    }
}
