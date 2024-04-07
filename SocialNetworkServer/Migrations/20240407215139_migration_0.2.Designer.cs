﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SocialNetworkServer.SocNetworkDBContext;

#nullable disable

namespace SocialNetworkServer.Migrations
{
    [DbContext(typeof(SocialNetworkDBContext))]
    [Migration("20240407215139_migration_0.2")]
    partial class migration_02
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SocialNetworkServer.SocNetworkDBContext.Entities.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PostPermissions")
                        .HasColumnType("int");

                    b.HasKey("GroupId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SocialNetworkServer.SocNetworkDBContext.Entities.GroupSubscription", b =>
                {
                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.Property<int>("SubscribedToGroupId")
                        .HasColumnType("int");

                    b.HasKey("SubscriberId", "SubscribedToGroupId");

                    b.HasIndex("SubscribedToGroupId");

                    b.ToTable("UserGroupSubscriptions", (string)null);
                });

            modelBuilder.Entity("SocialNetworkServer.SocNetworkDBContext.Entities.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PostId");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId", "GroupId", "IsDeleted");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("SocialNetworkServer.SocNetworkDBContext.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RegistrationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValue("New");

                    b.Property<string>("UserSurname")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValue("User");

                    b.HasKey("UserId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("SocialNetworkServer.SocNetworkDBContext.Entities.UserSubscription", b =>
                {
                    b.Property<int>("UserSubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("SubscribedToUserId")
                        .HasColumnType("int");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.HasKey("UserSubscriptionId");

                    b.HasIndex("SubscribedToUserId");

                    b.HasIndex("SubscriberId");

                    b.ToTable("UserSubscriptions", (string)null);
                });

            modelBuilder.Entity("SocialNetworkServer.SocNetworkDBContext.Entities.Group", b =>
                {
                    b.HasOne("SocialNetworkServer.SocNetworkDBContext.Entities.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("SocialNetworkServer.SocNetworkDBContext.Entities.GroupSubscription", b =>
                {
                    b.HasOne("SocialNetworkServer.SocNetworkDBContext.Entities.Group", "SubscribedToGroup")
                        .WithMany("Subscribers")
                        .HasForeignKey("SubscribedToGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialNetworkServer.SocNetworkDBContext.Entities.User", "Subscriber")
                        .WithMany("SubscribedGroups")
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubscribedToGroup");

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("SocialNetworkServer.SocNetworkDBContext.Entities.Post", b =>
                {
                    b.HasOne("SocialNetworkServer.SocNetworkDBContext.Entities.Group", "Group")
                        .WithMany("Posts")
                        .HasForeignKey("GroupId");

                    b.HasOne("SocialNetworkServer.SocNetworkDBContext.Entities.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialNetworkServer.SocNetworkDBContext.Entities.UserSubscription", b =>
                {
                    b.HasOne("SocialNetworkServer.SocNetworkDBContext.Entities.User", "SubscribedToUser")
                        .WithMany("Followers")
                        .HasForeignKey("SubscribedToUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialNetworkServer.SocNetworkDBContext.Entities.User", "Subscriber")
                        .WithMany("Followings")
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubscribedToUser");

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("SocialNetworkServer.SocNetworkDBContext.Entities.Group", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Subscribers");
                });

            modelBuilder.Entity("SocialNetworkServer.SocNetworkDBContext.Entities.User", b =>
                {
                    b.Navigation("Followers");

                    b.Navigation("Followings");

                    b.Navigation("Posts");

                    b.Navigation("SubscribedGroups");
                });
#pragma warning restore 612, 618
        }
    }
}
