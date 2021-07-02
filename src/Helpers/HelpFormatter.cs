
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using Hexa.Attributes;

namespace Michi.Helpers
{
    /// <summary>
    /// Default CommandsNext help formatter.
    /// </summary>
    public class MichiHelpFormatter : BaseHelpFormatter
    {
        public DiscordEmbedBuilder EmbedBuilder { get; }
        private Command Command { get; set; }

        /// <summary>
        /// Creates a new default help formatter.
        /// </summary>
        /// <param name="ctx">Context in which this formatter is being invoked.</param>
        public MichiHelpFormatter(CommandContext ctx)
            : base(ctx)
        {
            var hEmbed = new MichiEmbed(ctx, "michi help");
            this.EmbedBuilder = hEmbed.embed;
        }

        /// <summary>
        /// Sets the command this help message will be for.
        /// </summary>
        /// <param name="command">Command for which the help message is being produced.</param>
        /// <returns>This help formatter.</returns>
        public override BaseHelpFormatter WithCommand(Command command)
        {
            this.Command = command;
            this.EmbedBuilder.AddField("Description", command.Description ?? "none", true);
            this.EmbedBuilder.Title = $"help for ``{command.Name}``";
            var methods = command.Module.ModuleType.GetMethods().Where(m => m.ReturnType == typeof(System.Threading.Tasks.Task) && m.CustomAttributes.Any(x => x.AttributeType == typeof(DSharpPlus.CommandsNext.Attributes.CommandAttribute)));
            var thing = methods.Where(m => m.GetCustomAttributes(typeof(HelpHideAttribute), true).Any());
            var types = thing.Select(x => x.GetParameters().Select(y => y.ParameterType).Skip(1));
            if (command.Overloads?.Any() == true)
            {
                var sb = new StringBuilder();

                foreach (var ovl in command.Overloads.OrderByDescending(x => x.Priority))
                {
                    var ovlTypes = ovl.Arguments.Select(x => x.Type);
                    var ovlSelect = ovlTypes.Select(y => y.FullName).OrderBy(x => x);
                    if (types.Any(x => !((x.Select(y => y.FullName).OrderBy(x => x)).Except(ovlSelect).Any()) && !(ovlSelect.Except(x.Select(y => y.FullName).OrderBy(x => x))).Any()))
                        continue;

                    sb.Append('`').Append(command.QualifiedName);

                    foreach (var arg in ovl.Arguments)
                        sb.Append(arg.IsOptional || arg.IsCatchAll ? " [" : " <").Append(arg.Name).Append(arg.IsCatchAll ? "..." : "").Append(arg.IsOptional || arg.IsCatchAll ? ']' : '>');

                    sb.Append("`\n");
                }

                this.EmbedBuilder.AddField("Usage", sb.ToString().Trim(), false);
            }
            // }

            if (command.Aliases?.Any() == true)
                this.EmbedBuilder.AddField("Aliases", string.Join(", ", command.Aliases.Select(Formatter.InlineCode)), false);

            return this;
        }

        /// <summary>
        /// Sets the subcommands for this command, if applicable. This method will be called with filtered data.
        /// </summary>
        /// <param name="subcommands">Subcommands for this command group.</param>
        /// <returns>This help formatter.</returns>
        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            if (this.Command is null)
            {
                var bot_info = new StringBuilder();
                var prefix = Program.PREFIX;
                prefix = Regex.Replace(prefix, @"[\\\*\~_>`]", (Match m) => $"{m.Value}\u200B");
                bot_info.Append($"``{prefix}``: prefix\n");
                bot_info.Append($"{this.Context.Client.CurrentUser.Mention}: mention me for help\n");
                this.EmbedBuilder.Title = "bot info";
                this.EmbedBuilder.Description = bot_info.ToString().Trim();
                this.EmbedBuilder.AddField("important commands", $"use ``{prefix}createpoll`` to create a poll\nuse ``{prefix}createvote`` to create a simple vote", true);
                // this.EmbedBuilder.AddField("bugs", $"report any bugs using the ``{prefix}bugreport`` command to help improve michi", false);
                var command_list = new StringBuilder();
                int i = 0;
                foreach (var command in subcommands)
                {
                    command_list.Append($"``{command.Name}`` ");
                    if (i % 3 == 0)
                        command_list.Append("\n");
                    i++;
                }
                this.EmbedBuilder.AddField("Commands", command_list.ToString().Trim());
            }
            return this;
        }
        /// <summary>
        /// Construct the help message.
        /// </summary>
        /// <returns>Data for the help message.</returns>
        public override CommandHelpMessage Build()
        {
            return new CommandHelpMessage(embed: this.EmbedBuilder.Build());
        }
    }
}