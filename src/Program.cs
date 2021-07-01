using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Michi
{
    static class Program
    {
        public static string DBSTRING { get; private set; }
        public static string TOKEN { get; private set; }
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
            });
            var services = new ServiceCollection().
                AddSingleton<Michi.DB.PollContext>().
                BuildServiceProvider();
            var slash = discord.UseSlashCommands(new()
            {
                Services = services
            });
            slash.RegisterCommands<Modules.ButtonSlash>(844754896358998018);
            discord.UseInteractivity(new()
            {
                PaginationBehaviour = PaginationBehaviour.Ignore,
                PollBehaviour = PollBehaviour.DeleteEmojis,
                ResponseBehavior = InteractionResponseBehavior.Ack,
                Timeout = TimeSpan.FromMinutes(2)
            });
            discord.ComponentInteractionCreated += async (client, args) =>
                await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
