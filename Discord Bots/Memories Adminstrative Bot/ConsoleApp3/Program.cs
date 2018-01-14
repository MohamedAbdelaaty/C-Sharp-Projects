using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using ConsoleApp3.Modules.Public;

namespace ConsoleApp3
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

            //Initialize command handling.
            await commandInitiation();

            //Connect bot to Discord.
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

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
            var message = messageParam as SocketUserMessage;

            if(message == null)
            {
                return;
            }

            int argPos = 1;

            if(!(message.HasStringPrefix("M)", ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos)))
            {
                return;
            }

            var context = new CommandContext(client, message);

            var result = await commands.ExecuteAsync(context, argPos, map);

            if(!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        public async Task antiSpam()
        {

        }
    }
}
