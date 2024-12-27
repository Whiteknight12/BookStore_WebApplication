using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.Models.Models
{
    public class APIModel
    {
        private readonly HttpClient _httpClient;
        private IWebHostEnvironment _wwwroot;
        public APIModel(IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = new HttpClient();
            _wwwroot = webHostEnvironment;
        }

        public async Task<string> filecontent()
        {
            string webrootpath = _wwwroot.WebRootPath;
            string filepath = Path.Combine(webrootpath, "TrainingData.txt");
            string fileContent = await File.ReadAllTextAsync(filepath);
            return fileContent;
        }

        public async Task<string> GenerateContentAsync(string apiKey, string inputText)
        {
            string data = await filecontent();
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";
            var requestBody = new
            {
                contents = new[]
                {
                new { role = "user", parts = new[] { new { text = "Xin chào\n" } } },
                new { role = "model", parts = new[] { new { text = "Chào khách hàng thân yêu." } } },
                new { role = "user", parts = new[] { new { text = inputText } } }
            },
                systemInstruction = new
                {
                    role = "user",
                    parts = new[] { new { text = data } }
                },
                generationConfig = new
                {
                    temperature = 2,
                    topK = 40,
                    topP = 0.95,
                    maxOutputTokens = 8000,
                    responseMimeType = "text/plain"
                }
            };
            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }
    }
}
    

