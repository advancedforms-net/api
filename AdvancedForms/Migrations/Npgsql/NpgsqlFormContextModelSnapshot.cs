﻿// <auto-generated />
using System;
using AdvancedForms.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AdvancedForms.Migrations.Npgsql
{
    [DbContext(typeof(NpgsqlFormContext))]
    partial class NpgsqlFormContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AdvancedForms.Models.Form", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("UseCodes")
                        .HasColumnType("boolean");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Forms");
                });

            modelBuilder.Entity("AdvancedForms.Models.Preset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("FormId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TemplateId")
                        .HasColumnType("uuid");

                    b.Property<string>("ValuesJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FormId");

                    b.HasIndex("TemplateId");

                    b.ToTable("Presets");
                });

            modelBuilder.Entity("AdvancedForms.Models.PresetTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("FormId")
                        .HasColumnType("uuid");

                    b.Property<string>("ValuesJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FormId");

                    b.ToTable("PresetTemplates");
                });

            modelBuilder.Entity("AdvancedForms.Models.Response", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Creation")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PresetId")
                        .HasColumnType("uuid");

                    b.Property<string>("ValuesJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PresetId");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("AdvancedForms.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AdvancedForms.Models.Form", b =>
                {
                    b.HasOne("AdvancedForms.Models.User", null)
                        .WithMany("Forms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AdvancedForms.Models.Preset", b =>
                {
                    b.HasOne("AdvancedForms.Models.Form", "Form")
                        .WithMany("Presets")
                        .HasForeignKey("FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AdvancedForms.Models.PresetTemplate", "Template")
                        .WithMany("Presets")
                        .HasForeignKey("TemplateId");

                    b.Navigation("Form");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("AdvancedForms.Models.PresetTemplate", b =>
                {
                    b.HasOne("AdvancedForms.Models.Form", "Form")
                        .WithMany("PresetTemplates")
                        .HasForeignKey("FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Form");
                });

            modelBuilder.Entity("AdvancedForms.Models.Response", b =>
                {
                    b.HasOne("AdvancedForms.Models.Preset", "Preset")
                        .WithMany("Responses")
                        .HasForeignKey("PresetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Preset");
                });

            modelBuilder.Entity("AdvancedForms.Models.Form", b =>
                {
                    b.Navigation("PresetTemplates");

                    b.Navigation("Presets");
                });

            modelBuilder.Entity("AdvancedForms.Models.Preset", b =>
                {
                    b.Navigation("Responses");
                });

            modelBuilder.Entity("AdvancedForms.Models.PresetTemplate", b =>
                {
                    b.Navigation("Presets");
                });

            modelBuilder.Entity("AdvancedForms.Models.User", b =>
                {
                    b.Navigation("Forms");
                });
#pragma warning restore 612, 618
        }
    }
}
