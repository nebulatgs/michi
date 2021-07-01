using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Michi.DB
{
    public partial class PollContext : DbContext
    {
        public PollContext()
        {
        }

        public PollContext(DbContextOptions<PollContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Poll> Polls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql(Michi.Program.DBSTRING, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.25-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<Poll>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ChannelId).HasColumnName("channelId");

                entity.Property(e => e.Choice)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("choice");

                entity.Property(e => e.ChoiceEmoji)
                    .HasColumnType("text")
                    .HasColumnName("choiceEmoji");

                entity.Property(e => e.GuildId).HasColumnName("guildId");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("value");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
