using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;


namespace AntiAd
{
    class Program
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private DependencyMap map;

        static void Main(string[] args)
        {
            new Program().MyBotStart().GetAwaiter().GetResult();
        }

        public async Task MyBotStart()
        {
            //Declaring variables.
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
            });
            map = new DependencyMap();
            client.Log += Log;
            string token = "";
            commands = new CommandService();

            // Server events start here //
            client.MessageReceived += messageSent;
            client.UserUpdated += updated;
            // Server events end here //

            //Initialize command handling.
            await commandInitiation();

            //Connect bot to Discord.
            await client.LoginAsync(TokenType.Bot, token);
            client.SetGameAsync("Anti-Ad bot");
            await client.StartAsync();
            // await client.SetGameAsync("Memories", null, StreamType.NotStreaming);

            //Prevents bot from dying naturally to end of code.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task commandInitiation()
        {
            client.MessageReceived += CommandHandler;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task CommandHandler(SocketMessage messageParam)
        {
            return;
        }

        private async Task messageSent(SocketMessage msg) /*Creates the method*/
        {
            var user = msg.Author; /*Sets variable user to msg.author*/

            // What to filter/look for in a message
            // You can add more onto the list without needed to change any of the code (besides the filter array itself)
            string[] filter = new string[] {"discord.gg", "twitch.tv", "youtube.com/channel", "goo.gl", "bit.ly", "tinyurl.com", "twitchdot"} ;

            string input = msg.ToString();

            input = input.Replace(" ", String.Empty);

            foreach (var fil in filter) {
                if (input.ToUpper().Contains(fil.ToUpper()) || ((input.ToUpper().Contains("twitch".ToUpper()) && (input.ToUpper().Contains("tv".ToUpper())))))
                {
                    await msg.DeleteAsync();
                    break;
                }
            }
        }
    }
}
