using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptobot.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Configuration;

namespace Cryptobot.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IMarket client;
        public MessagesController(IConfiguration configuration, IMarket client)
        {
            this.configuration = configuration;
            this.client = client;
        }

        [Authorize(Roles = "Bot")]
        [HttpPost]
        public async Task<OkResult> Post([FromBody] Activity activity)
        {
            var appCredentials = new MicrosoftAppCredentials(this.configuration);
            var client = new ConnectorClient(new Uri(activity.ServiceUrl), appCredentials);
            var reply = activity.CreateReply();
            if (activity.Type == ActivityTypes.Message)
            {
                //var markets = (await this.client.AllMarkets()).ToList();

                //var requestedMarket = markets
                //                        .Where(w => activity.Text.Contains(w.Name))
                //                        .ToList();
                //if (requestedMarket.Count == 0)
                //{
                //    reply.Text = "Crypto monnaies prisent en charge: ";
                //    await client.Conversations.ReplyToActivityAsync(reply);
                //    reply = activity.CreateReply();
                //    markets.ForEach(market =>
                //    {
                //        reply.Text += $"{market.Name} ({market.LastPrice})<br>";
                //    });
                //    await client.Conversations.ReplyToActivityAsync(reply);
                //}
                //else
                //{
                //    reply.Text = "Voici les valeurs pour les marchés demandés";
                //    await client.Conversations.ReplyToActivityAsync(reply);

                //    requestedMarket.ForEach(async m =>
                //    {
                //        reply = activity.CreateReply();
                //        reply.Text = $"Pour le marché {m.Name} le dernier prix d'échange est de {m.LastPrice}";
                //        await client.Conversations.ReplyToActivityAsync(reply);
                //    });
                //}
            }
            return Ok();
        }
    }

}