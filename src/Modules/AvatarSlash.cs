using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
public class AvatarSlash : SlashCommandModule
{
    [SlashCommand("avatar", "Get someone's avatar")]
    public async Task AvatarCommand(InteractionContext ctx, [Option("user", "The user to get it for")] DiscordUser user = null)
    {
        user ??= ctx.Member;
    }
}