﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Steam_Achievements_Analysis_System.YourOutputDirectory;

#nullable disable

namespace Steam_Achievements_Analysis_System.Migrations
{
    [DbContext(typeof(SteamGameAchivmentContext))]
    partial class SteamGameAchivmentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Steam_Achievements_Analysis_System.YourOutputDirectory.Achievement", b =>
                {
                    b.Property<int>("AchievementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AchievementId"));

                    b.Property<string>("AchivmentName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("AppId")
                        .HasColumnType("int");

                    b.HasKey("AchievementId")
                        .HasName("PK__Achievem__276330C0B4B2C514");

                    b.HasIndex(new[] { "AppId", "AchivmentName" }, "UQ_Achievement")
                        .IsUnique();

                    b.ToTable("Achievements");
                });

            modelBuilder.Entity("Steam_Achievements_Analysis_System.YourOutputDirectory.AchievementPercentage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AchievementId")
                        .HasColumnType("int");

                    b.Property<double>("Percentage")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("PK__Achievem__3214EC072195B46B");

                    b.HasIndex("AchievementId");

                    b.ToTable("AchievementPercentages");
                });

            modelBuilder.Entity("Steam_Achievements_Analysis_System.YourOutputDirectory.Game", b =>
                {
                    b.Property<int>("AppId")
                        .HasColumnType("int");

                    b.Property<string>("GameName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("AppId")
                        .HasName("PK__Games__8E2CF7F9B62C1278");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Steam_Achievements_Analysis_System.YourOutputDirectory.Achievement", b =>
                {
                    b.HasOne("Steam_Achievements_Analysis_System.YourOutputDirectory.Game", "App")
                        .WithMany("Achievements")
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Achieveme__AppId__3A81B327");

                    b.Navigation("App");
                });

            modelBuilder.Entity("Steam_Achievements_Analysis_System.YourOutputDirectory.AchievementPercentage", b =>
                {
                    b.HasOne("Steam_Achievements_Analysis_System.YourOutputDirectory.Achievement", "Achievement")
                        .WithMany("AchievementPercentages")
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_AchievementPercentages_Achievements");

                    b.Navigation("Achievement");
                });

            modelBuilder.Entity("Steam_Achievements_Analysis_System.YourOutputDirectory.Achievement", b =>
                {
                    b.Navigation("AchievementPercentages");
                });

            modelBuilder.Entity("Steam_Achievements_Analysis_System.YourOutputDirectory.Game", b =>
                {
                    b.Navigation("Achievements");
                });
#pragma warning restore 612, 618
        }
    }
}
