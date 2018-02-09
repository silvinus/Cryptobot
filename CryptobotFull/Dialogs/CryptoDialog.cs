using Cryptobot.Domain;
using Cryptobot.Interface;
using CryptobotFull.Storage;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CryptobotFull.Dialogs
{
    [Serializable]
    public class CryptoDialog : IDialog<object>
    {
        private readonly ICryptoStorage store;
        private readonly IMarket market;
        private CryptoData data;

        public CryptoDialog(ICryptoStorage storage, IMarket market)
        {
            this.store = storage;
            this.market = market;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Donne moi ton pseudonyme");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            data = store.Create(message.Text);

            if (message.Text.Contains("convertion"))
            {
                await this.SendChangePreferenceMessageAsync(context);
            }

            else if (data.Currencies.Count > 0 || message.Text.ToLower().Contains("ajout"))
            {
                await this.SendCurrentCurrencies(context);
            }
            else
            {
                await this.SendAddCurrency(context);
            }
        }

        #region TODO: In ChangePreferenceDialog implementation
        private async Task SendChangePreferenceMessageAsync(IDialogContext context)
        {
            await context.PostAsync("EUR/USD/BTC ?");
            context.Wait(this.ReceivedChangePreferenceAsync);
        }

        private async Task ReceivedChangePreferenceAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (Enum.TryParse(message.Text, out ManagedConvertion convert)) {
                data.Preference = convert;
                if (data.Currencies.Count > 0)
                {
                    this.SendCurrentCurrencies(context);
                }
                else
                {
                    this.SendAddCurrency(context);
                }
            }
            else
            {
                await context.PostAsync($"Désolé je ne connais pas cette devise: {message.Text}");
                context.Wait(this.ReceivedChangePreferenceAsync);
            }
        }
        #endregion

        private async Task SendAddCurrency(IDialogContext context)
        {
            await context.PostAsync("Quelle valeur veux tu voir ?");
            context.Wait(this.ReceivedAddCurrency);
        }

        private async Task ReceivedAddCurrency(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            var marketResult = await this.market.Market(message.Text);

            if(marketResult == null)
            {
                await context.PostAsync("Cette devise est inconnue!");
                await this.SendAddCurrency(context);
            }

            await context.PostAsync($"{marketResult.Name} s'échange actuellement à une valeur de {marketResult.Convert(data.Preference)}");

            await context.PostAsync("Voulez vous ajouter cette valeur à votre liste de valeurs courantes ?");
            context.Wait(async (ctx,  res) => {
                var msg = await res;
                if(msg.Text.ToLower() == "oui")
                {
                    data.Currencies.Add(marketResult.Name);
                    context.Done(String.Empty);
                }
                else
                {
                    await this.SendCurrentCurrencies(context);
                }
            });
        }

        private async Task SendCurrentCurrencies(IDialogContext context)
        {
            data.Currencies
                .ForEach(async c =>
                {
                    var marketResult = await this.market.Market(c);
                    await context.PostAsync($"{marketResult.Name} s'échange actuellement à une valeur de {marketResult.Convert(data.Preference)}");
                });

            await context.PostAsync("Tous est bon ?");
            context.Wait(async (c, r) => c.Done(String.Empty));
        }
    }
}