using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tag.Gateway.Managers;
using Telegram.Bot.Types;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Tag.Gateway.Api
{
    public class Gateway(ILogger<Gateway> logger, IMessageManager messageManager)
    {
        private readonly ILogger<Gateway> _logger = logger;
        private readonly IMessageManager _messageManager = messageManager;

        [Function("Gateway")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] [FromBody]Update tgUpdate)
        {
            _logger.LogInformation($"Update received: {JsonConvert.SerializeObject(tgUpdate)}");

            await _messageManager.PostMessageAsync(tgUpdate);

            return new OkResult();
        }
    }
}
