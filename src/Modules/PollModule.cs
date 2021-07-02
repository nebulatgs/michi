using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Michi.DB;

namespace Michi.Modules
{
    public class PollModule : BaseCommandModule
    {
        public PollContext db { get; set; }
        [Command("createpoll")]
        [Aliases("startpoll", "create", "poll", "sp")]
        public async Task AvatarCommand(CommandContext ctx)
        {
            // await ctx.RespondAsync("Hi!");
            // await db.AddAsync(new Poll(){
            //     GuildId = ctx.Guild.Id,
            //     ChannelId = ctx.Channel.Id,
            //     Choice = "One",
            //     Value = "Two",
            // });
            // await db.SaveChangesAsync();
            await ctx.Guild.CreateApplicationCommandAsync(new DiscordApplicationCommand("test", "test"));
        }
    }
}