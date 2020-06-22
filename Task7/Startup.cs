using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Owin;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

[assembly: OwinStartup(typeof(Task7.Startup))]

namespace Task7
{

    public static class Bot
    {
        public static readonly TelegramBotClient Api = new TelegramBotClient("API Key");
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
