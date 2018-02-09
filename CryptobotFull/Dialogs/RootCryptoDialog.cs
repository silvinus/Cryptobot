using System;
using System.Threading.Tasks;
using Cryptobot.Interface;
using CryptobotFull.Storage;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace CryptobotFull.Dialogs
{
    [Serializable]
    public class RootCryptoDialog : IDialog<object>
    {
        private readonly ICryptoStorage store;
        private readonly IMarket market;

        public RootCryptoDialog(ICryptoStorage storage, IMarket market)
        {
            this.store = storage;
            this.market = market;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {length} characters...");

            //context.Wait(MessageReceivedAsync);

            //await this.SendWelcomeMessageAsync(context);
        }

        //private async Task SendWelcomeMessageAsync(IDialogContext context)
        //{
        //    await context.PostAsync("Hello... Je suis Cryptobot. Je peux te donner les valeurs de tes crypto monnaies");
        //    context.Call(new CryptoDialog(this.store, this.market), this.EndDialog);
        //}

        //private async Task EndDialog(IDialogContext context, IAwaitable<object> result)
        //{
        //    await this.SendWelcomeMessageAsync(context);
        //}
    }
}