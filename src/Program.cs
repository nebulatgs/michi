using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using Michi.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Michi
{
    static class Program
    {
        public static string DBSTRING { get; private set; }
        public static string TOKEN { get; private set; }
        public static readonly string PREFIX = "&";
        private static IConfiguration config;
        // public static 
        static void Configure()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(path: "config.json");
            config = builder.Build();

            if (Environment.GetEnvironmentVariable("PROD") is not null)
            {
                DBSTRING = Environment.GetEnvironmentVariable("DBSTRING");
                TOKEN = Environment.GetEnvironmentVariable("BOT_TOKEN");
                Environment.SetEnvironmentVariable("DBSTRING", null);
                Environment.SetEnvironmentVariable("BOT_TOKEN", null);
            }
            else
            {
                DBSTRING = config["DBSTRING"];
                TOKEN = config["TOKEN"];
            }
        }
        static async Task Main(string[] args)
        {
            Configure();
            var discord = new DiscordClient(new()
            {
                Token = TOKEN,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                MinimumLogLevel = LogLevel.Debug
            });
            var services = new ServiceCollection().
                AddSingleton<Michi.DB.PollContext>().
                BuildServiceProvider();
            var cnext = discord.UseCommandsNext(new()
            {
                StringPrefixes = new[] { PREFIX },
                EnableDms = true,
                Services = services
            });
            cnext.RegisterCommands(Assembly.GetExecutingAssembly());
            cnext.SetHelpFormatter<MichiHelpFormatter>();
            var slash = discord.UseSlashCommands(new() { Services = services });

            // discord.MessageCreated += (client, args) => args.Channel.SendMessageAsync("hi");

            slash.RegisterCommands<Modules.PollSlash>(844754896358998018);
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
