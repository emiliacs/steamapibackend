// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RyhmatyoBuuttiServer;

namespace RyhmatyoBuuttiServer.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210923085010_AddImageToGame")]
    partial class AddImageToGame
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("RyhmatyoBuuttiServer.Models.Developer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Developers")
                        .HasColumnType("text");

                    b.Property<long?>("GameId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Developer");
                });

            modelBuilder.Entity("RyhmatyoBuuttiServer.Models.Game", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("ReleaseYear")
                        .HasColumnType("integer");

                    b.Property<int>("SteamId")
                        .HasColumnType("integer");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("RyhmatyoBuuttiServer.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<long?>("GameId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("RyhmatyoBuuttiServer.Models.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long?>("GameId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Publisher");
                });

            modelBuilder.Entity("RyhmatyoBuuttiServer.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("ResetCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ResetCodeExpires")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("SteamId")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.Property<string>("VerificationCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("VerificationCodeExpires")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Verified")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RyhmatyoBuuttiServer.Models.Developer", b =>
                {
                    b.HasOne("RyhmatyoBuuttiServer.Models.Game", null)
                        .WithMany("Developers")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("RyhmatyoBuuttiServer.Models.Game", b =>
                {
                    b.HasOne("RyhmatyoBuuttiServer.Models.User", null)
                        .WithMany("Games")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("RyhmatyoBuuttiServer.Models.Genre", b =>
                {
                    b.HasOne("RyhmatyoBuuttiServer.Models.Game", null)
                        .WithMany("Genres")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("RyhmatyoBuuttiServer.Models.Publisher", b =>
                {
                    b.HasOne("RyhmatyoBuuttiServer.Models.Game", null)
                        .WithMany("Publishers")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("RyhmatyoBuuttiServer.Models.Game", b =>
                {
                    b.Navigation("Developers");

                    b.Navigation("Genres");

                    b.Navigation("Publishers");
                });

            modelBuilder.Entity("RyhmatyoBuuttiServer.Models.User", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
