using System.Text;
using System.Text.Json;

namespace VaggouAPI
{
    public class IAChatService : IIAChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "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";
        private readonly string apiModel = "deepseek/deepseek-r1-0528:free";
        private readonly string _referer = "http://localhost:5077";

        public IAChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SendMensageAsync(string mensagem)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {_apiKey}"); 
            request.Headers.Referrer = new Uri(_referer);
            request.Headers.Add("X-Title", "Vaggou IA");

            var body = new
            {
                model = apiModel, 
                messages = new[]
                {
                    new { role = "system", content = "Você é um assistente." },
                    new { role = "user", content = mensagem }
                }
            };

            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao chamar a IA: {content}");
            }

            return content;
        }
    }
}
