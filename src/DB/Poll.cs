using System;
using System.Collections.Generic;

#nullable disable

namespace Michi.DB
{
    public partial class Poll
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public ulong ChannelId { get; set; }
        public string ChoiceEmoji { get; set; }
        public string Choice { get; set; }
        public string Value { get; set; }
    }
}
