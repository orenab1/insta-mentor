﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220206124201_Communities_updated_Desc")]
    partial class Communities_updated_Desc
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("DAL.Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AboutMe")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("BLOB");

                    b.Property<int?>("PhotoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DAL.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CommentorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuestionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CommentorId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("DAL.Entities.Community", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BestTimeToGetAnswer")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int>("CreatorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Communities");
                });

            modelBuilder.Entity("DAL.Entities.EmailPrefrence", b =>
                {
                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("MyQuestionReceivedNewComment")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("MyQuestionReceivedNewOffer")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("OnlyNotifyOnCommunityQuestionAsked")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("QuestionAskedOnMyTags")
                        .HasColumnType("INTEGER");

                    b.HasKey("AppUserId");

                    b.ToTable("EmailPrefrences");
                });

            modelBuilder.Entity("DAL.Entities.Offer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int>("OffererId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuestionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OffererId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("DAL.Entities.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PublicId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("DAL.Entities.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AskerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Body")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Header")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPayed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSolved")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AskerId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("DAL.Entities.QuestionsCommunities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CommunityId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuestionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CommunityId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionsCommunities");
                });

            modelBuilder.Entity("DAL.Entities.QuestionsTags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuestionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TagId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("TagId");

                    b.ToTable("QuestionsTags");
                });

            modelBuilder.Entity("DAL.Entities.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsRevieweeAnswerer")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuestionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RevieweeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReviewerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("RevieweeId");

                    b.HasIndex("ReviewerId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("DAL.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsApproved = true,
                            Text = "SQL"
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsApproved = true,
                            Text = "Python"
                        },
                        new
                        {
                            Id = 3,
                            Created = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsApproved = true,
                            Text = "React"
                        },
                        new
                        {
                            Id = 4,
                            Created = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsApproved = true,
                            Text = "Angular"
                        });
                });

            modelBuilder.Entity("DAL.Entities.UsersCommunities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CommunityId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("CommunityId");

                    b.ToTable("UsersCommunities");
                });

            modelBuilder.Entity("DAL.Entities.UsersTags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TagId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("TagId");

                    b.ToTable("UsersTags");
                });

            modelBuilder.Entity("DAL.Entities.AppUser", b =>
                {
                    b.HasOne("DAL.Entities.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");

                    b.Navigation("Photo");
                });

            modelBuilder.Entity("DAL.Entities.Comment", b =>
                {
                    b.HasOne("DAL.Entities.AppUser", "Commentor")
                        .WithMany()
                        .HasForeignKey("CommentorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Question", "Question")
                        .WithMany("Comments")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Commentor");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("DAL.Entities.Community", b =>
                {
                    b.HasOne("DAL.Entities.AppUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("DAL.Entities.EmailPrefrence", b =>
                {
                    b.HasOne("DAL.Entities.AppUser", "AppUser")
                        .WithOne("EmailPrefrence")
                        .HasForeignKey("DAL.Entities.EmailPrefrence", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("DAL.Entities.Offer", b =>
                {
                    b.HasOne("DAL.Entities.AppUser", "Offerer")
                        .WithMany()
                        .HasForeignKey("OffererId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Question", "Question")
                        .WithMany("Offers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Offerer");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("DAL.Entities.Question", b =>
                {
                    b.HasOne("DAL.Entities.AppUser", "Asker")
                        .WithMany("Questions")
                        .HasForeignKey("AskerId");

                    b.Navigation("Asker");
                });

            modelBuilder.Entity("DAL.Entities.QuestionsCommunities", b =>
                {
                    b.HasOne("DAL.Entities.Community", "Community")
                        .WithMany("Questions")
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Question", "Question")
                        .WithMany("Communities")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Community");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("DAL.Entities.QuestionsTags", b =>
                {
                    b.HasOne("DAL.Entities.Question", "Question")
                        .WithMany("Tags")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Tag", "Tag")
                        .WithMany("Questions")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("DAL.Entities.Review", b =>
                {
                    b.HasOne("DAL.Entities.Question", "Question")
                        .WithMany("Reviews")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.AppUser", "Reviewee")
                        .WithMany("ReviewsReceived")
                        .HasForeignKey("RevieweeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DAL.Entities.AppUser", "Reviewer")
                        .WithMany("ReviewsGiven")
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Reviewee");

                    b.Navigation("Reviewer");
                });

            modelBuilder.Entity("DAL.Entities.Tag", b =>
                {
                    b.HasOne("DAL.Entities.AppUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("DAL.Entities.UsersCommunities", b =>
                {
                    b.HasOne("DAL.Entities.AppUser", "AppUser")
                        .WithMany("Communities")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Community", "Community")
                        .WithMany("Users")
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Community");
                });

            modelBuilder.Entity("DAL.Entities.UsersTags", b =>
                {
                    b.HasOne("DAL.Entities.AppUser", "AppUser")
                        .WithMany("Tags")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Tag", "Tag")
                        .WithMany("Users")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("DAL.Entities.AppUser", b =>
                {
                    b.Navigation("Communities");

                    b.Navigation("EmailPrefrence");

                    b.Navigation("Questions");

                    b.Navigation("ReviewsGiven");

                    b.Navigation("ReviewsReceived");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("DAL.Entities.Community", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("DAL.Entities.Question", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Communities");

                    b.Navigation("Offers");

                    b.Navigation("Reviews");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("DAL.Entities.Tag", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
