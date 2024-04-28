using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Steam_Achievements_Analysis_System.YourOutputDirectory;

public partial class SteamGameAchivmentContext : DbContext
{
    public SteamGameAchivmentContext()
    {
    }

    public SteamGameAchivmentContext(DbContextOptions<SteamGameAchivmentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<AchievementPercentage> AchievementPercentages { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ASUSF15\\SQLEXPRESS;Database=SteamGameAchivment;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.AchievementId).HasName("PK__Achievem__276330C0B4B2C514");

            entity.HasIndex(e => new { e.AppId, e.AchivmentName }, "UQ_Achievement").IsUnique();

            entity.Property(e => e.AchivmentName).HasMaxLength(255);

            entity.HasOne(d => d.App).WithMany(p => p.Achievements)
                .HasForeignKey(d => d.AppId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Achieveme__AppId__3A81B327");
        });

        modelBuilder.Entity<AchievementPercentage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Achievem__3214EC072195B46B");

            entity.HasOne(d => d.Achievement).WithMany(p => p.AchievementPercentages)
                .HasForeignKey(d => d.AchievementId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_AchievementPercentages_Achievements");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.AppId).HasName("PK__Games__8E2CF7F9B62C1278");

            entity.Property(e => e.AppId).ValueGeneratedNever();
            entity.Property(e => e.GameName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
