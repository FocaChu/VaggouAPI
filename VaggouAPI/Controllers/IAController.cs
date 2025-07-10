using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class IAController : ControllerBase
    {
        private readonly IIAChatService _service;

        public IAController(IIAChatService service)
        {
            _service = service;
        }

        [HttpPost("prompt")]
        public async Task<IActionResult> EnviarPrompt([FromBody] PromptRequest request)
        {
            try
            {
                var resposta = await _service.SendMensageAsync(request.Mensagem);
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}