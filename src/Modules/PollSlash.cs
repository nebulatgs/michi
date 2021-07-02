using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Michi.Modules
{
    public class PollSlash : SlashCommandModule
    {
        [SlashCommand("create", "Create a new poll")]
        public async Task CreateCommand(InteractionContext ctx)
        {
            DiscordFollowupMessageBuilder builder = new();
            builder.Content = "hi";
            await ctx.FollowUpAsync(builder);
        }
    }
}