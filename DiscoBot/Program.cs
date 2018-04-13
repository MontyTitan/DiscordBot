using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace DiscoBot
{
    public class Program
    {
        DiscordSocketClient _client;
        CommandHandler _handler;
        public static Dictionary<string, string> globals = new Dictionary<string, string>();
        public static string roleName;
        public static string channelTag;
        public static string numberOfMessages;

        //static void Main(string[] args)
        //=> new Program().StartAsync().GetAwaiter().GetResult();

        static void Main(string[] args)
        {
            if (!File.Exists("settingsAdmin.json")) Environment.Exit(0);
            string json = File.ReadAllText("settingsAdmin.json");
            globals = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            roleName = globals["roleName"];
            channelTag = globals["channelTag"];
            numberOfMessages = globals["numberOfMessages"];
            new Program().StartAsync().GetAwaiter().GetResult();
        }

        public async Task StartAsync()
        {
            /*string name = "Bryant";
            string botName = "DiscoBot";
            string message = Utilities.GetFormattedAlert("WELCOME_&NAME_&BOTNAME", name, botName);
            Console.WriteLine(message);
            Console.ReadLine();
            return;*/
            if (Config.bot.token == "" || Config.bot.token == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}
