using System;

using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Michi.Helpers
{
    public class MichiEmbed
    {   
        public DiscordEmbedBuilder embed;
        public DiscordEmbed Build()
        {
            return embed.Build();
        }
        public MichiEmbed(CommandContext Context, string Title) : base()
        {
            embed = new DiscordEmbedBuilder
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = Context.Client.CurrentUser.AvatarUrl,
                    Name = Title
                },
                Color = new DiscordColor(0xC60B7C),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = Context.Message.Author.AvatarUrl,
                    Text = $"{Context.Message.Author.Username}#{Context.Message.Author.Discriminator}"
                }
            };
            embed.Build();
        }

        public MichiEmbed(InteractionContext Context, string Title) : base()
        {
            embed = new DiscordEmbedBuilder
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = Context.Client.CurrentUser.AvatarUrl,
                    Name = Title
                },
                Color = new DiscordColor(0xd63031),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = Context.User.AvatarUrl,
                    Text = $"{Context.User.Username}#{Context.User.Discriminator}"
                }
            };
            embed.Build();
        }

        public MichiEmbed WithFooter(string footer_text)
        {
            embed.Footer.Text = $"{footer_text}  •  {embed.Footer.Text}";
            return this;
        }
    }
}