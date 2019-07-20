namespace BuFaKAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Telegram.Bot;

    public class TelegramBot
    {
        private readonly TelegramBotClient botClient;

        public TelegramBot()
        {
            this.botClient = new TelegramBotClient("614614065:AAHB7zctCFcRwatfknOv2S1LHswnVTGPVQo");
        }

        public async void SendTextMessage(string message)
        {
            await this.botClient.SendMessageAsync(chatId: "-341694652", text: message);
        }
    }
}
