﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RepositoryLayer.Context;

#nullable disable

namespace RepositoryLayer.Migrations
{
    [DbContext(typeof(FundooDBContext))]
    partial class FundooDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LabelEntityNotesEntity", b =>
                {
                    b.Property<int>("LabelsLabelId")
                        .HasColumnType("int");

                    b.Property<int>("NotesId")
                        .HasColumnType("int");

                    b.HasKey("LabelsLabelId", "NotesId");

                    b.HasIndex("NotesId");

                    b.ToTable("LabelEntityNotesEntity");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.CollaboratorEntity", b =>
                {
                    b.Property<int>("CollaboratorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CollaboratorId"), 1L, 1);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NotesId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CollaboratorId");

                    b.HasIndex("NotesId");

                    b.HasIndex("UserId");

                    b.ToTable("Collaborators");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.LabelEntity", b =>
                {
                    b.Property<int>("LabelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LabelId"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("LabelId");

                    b.HasIndex("userId");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.NotesEntity", b =>
                {
                    b.Property<int>("NotesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotesId"), 1L, 1);

                    b.Property<string>("Colour")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsArchive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTrash")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Remainder")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("NotesId");

                    b.HasIndex("UserId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.UserEntity", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LabelEntityNotesEntity", b =>
                {
                    b.HasOne("RepositoryLayer.Entities.LabelEntity", null)
                        .WithMany()
                        .HasForeignKey("LabelsLabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RepositoryLayer.Entities.NotesEntity", null)
                        .WithMany()
                        .HasForeignKey("NotesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepositoryLayer.Entities.CollaboratorEntity", b =>
                {
                    b.HasOne("RepositoryLayer.Entities.NotesEntity", "NotesCollaborator")
                        .WithMany()
                        .HasForeignKey("NotesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RepositoryLayer.Entities.UserEntity", "UserCollaborator")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NotesCollaborator");

                    b.Navigation("UserCollaborator");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.LabelEntity", b =>
                {
                    b.HasOne("RepositoryLayer.Entities.UserEntity", "UserLabel")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserLabel");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.NotesEntity", b =>
                {
                    b.HasOne("RepositoryLayer.Entities.UserEntity", "NotesUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NotesUser");
                });
#pragma warning restore 612, 618
        }
    }
}
