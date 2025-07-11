using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace VaggouAPI
{
    public class IAChatService : IIAChatService
    {
        private readonly HttpClient _httpClient;
        private readonly Db _context;
        private readonly string _apiKey = "Your-API-Key";
        private readonly string apiModel = "deepseek/deepseek-r1-0528:free";
        private readonly string _referer = "http://localhost:5077";

        public IAChatService(HttpClient httpClient, Db context)
        {
            _httpClient = httpClient;
            _context = context;
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

        public async Task<MonthlyReport> GenerateReportAnalysisAsync(Guid id)
        {
            var report = await _context.MonthlyReports.FindAsync(id)
                ?? throw new NotFoundException("Report not found.");

            var reviews = await _context.Reviews
                .Where(p => p.ParkingLotId == report.ParkingLotId &&
                p.CreatedAt.Month == report.Month && p.CreatedAt.Year == report.Year
                )
                .Select(p => p.Comment)
                .ToListAsync();

            var data = new
            {
                mes = report.Month,
                totalReservas = report.TotalReservations,
                totalCancelamentos = report.TotalCancelations,
                totalLucro = report.TotalRevenue,
                mudancaAvaliacao = report.ScoreChange,
                horarioPico = report.PeakHours,
                comentarios = reviews
            };

            var json = JsonSerializer.Serialize(data);

            var prompt = $@"
            Com base no seguinte JSON de relatório mensal, escreva um resumo textual amigável e claro, como se estivesse explicando para o dono do estacionamento.
            Não use emojis ou girias, não fale nada que não seja o resumo solicitado, não se apresente.:
            {json}
            ";

            var response = await SendMensageAsync(prompt);

            using var doc = JsonDocument.Parse(response);
            var iaText = doc.RootElement
                             .GetProperty("choices")[0]
                             .GetProperty("message")
                             .GetProperty("content")
                             .GetString();

            report.AiAnalysis = iaText;
            await _context.SaveChangesAsync();

            return report;
        }
    }
}
