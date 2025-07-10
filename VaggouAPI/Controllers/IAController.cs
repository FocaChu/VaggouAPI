using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace VaggouAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class IAController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public IAController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("chatbot")]
        public async Task<IActionResult> SendPrompt([FromBody] PromptRequest request)
        {
            var apiKey = "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";

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions");
            httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");
            httpRequest.Headers.Add("HTTP-Referer", "http://localhost:5077"); 
            httpRequest.Headers.Add("X-Title", "Vaggou chatbot");

            var body = new
            {
                model = "deepseek/deepseek-r1-0528:free", 
                messages = new[]
                {
                 new { role = "user", content = request.Mensagem }
                }
            };

            httpRequest.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");  

            var response = await _httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Erro ao chamar a IA: {erro}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpPost("report_analysis")]
        public async Task<IActionResult> SendReportAnalysis([FromBody] PromptRequest request)
        {
            var apiKey = "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";

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions");
            httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");
            httpRequest.Headers.Add("HTTP-Referer", "http://localhost:5077");
            httpRequest.Headers.Add("X-Title", "Vaggou chatbot");

            var body = new
            {
                model = "deepseek/deepseek-r1-0528:free",
                messages = new[]
                {
                 new { role = "user", content = request.Mensagem }
                }
            };

            httpRequest.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Erro ao chamar a IA: {erro}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
    }

    public class PromptRequest
    {
        public string Mensagem { get; set; }
    }

}
