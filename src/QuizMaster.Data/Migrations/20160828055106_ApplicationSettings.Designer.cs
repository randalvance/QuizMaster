using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using QuizMaster.Data;

namespace QuizMaster.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160828055106_ApplicationSettings")]
    partial class ApplicationSettings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("QuizMaster.Models.Answer", b =>
                {
                    b.Property<Guid>("AnswerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AnswerText");

                    b.Property<int>("OrderNumber");

                    b.Property<Guid>("QuestionId");

                    b.HasKey("AnswerId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("QuizMaster.Models.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("QuizMaster.Models.ApplicationSetting", b =>
                {
                    b.Property<Guid>("ApplicationSettingId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApplicationSettingValueType");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("ApplicationSettingId");

                    b.HasAlternateKey("Name")
                        .HasName("UQ_ApplicationSetting_Name");

                    b.ToTable("ApplicationSettings");
                });

            modelBuilder.Entity("QuizMaster.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("QuizMaster.Models.Question", b =>
                {
                    b.Property<Guid>("QuestionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("QuestionText");

                    b.HasKey("QuestionId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("QuizMaster.Models.QuestionChoice", b =>
                {
                    b.Property<Guid>("QuestionChoiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Choice");

                    b.Property<int>("DisplayOrder");

                    b.Property<Guid>("QuestionId");

                    b.HasKey("QuestionChoiceId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionChoices");
                });

            modelBuilder.Entity("QuizMaster.Models.Quiz", b =>
                {
                    b.Property<Guid>("QuizId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AnswersOrderImportant");

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Instructions")
                        .IsRequired();

                    b.Property<DateTime>("ModifyDate");

                    b.Property<string>("QuizGroupCode");

                    b.Property<Guid>("QuizGroupId");

                    b.Property<int>("QuizType");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("QuizId");

                    b.HasAlternateKey("Code")
                        .HasName("UQ_Quiz_Code");

                    b.HasIndex("QuizGroupId");

                    b.ToTable("Quizes");
                });

            modelBuilder.Entity("QuizMaster.Models.QuizCategory", b =>
                {
                    b.Property<Guid>("QuizCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("QuizCategoryId");

                    b.HasAlternateKey("Code")
                        .HasName("UQ_QuizCategory_Code");

                    b.ToTable("QuizCategories");
                });

            modelBuilder.Entity("QuizMaster.Models.QuizChoice", b =>
                {
                    b.Property<Guid>("QuizChoiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Choice");

                    b.Property<int>("DisplayOrder");

                    b.Property<Guid>("QuizId");

                    b.HasKey("QuizChoiceId");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizChoices");
                });

            modelBuilder.Entity("QuizMaster.Models.QuizGroup", b =>
                {
                    b.Property<Guid>("QuizGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArticleContentHtml");

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid>("QuizCategoryId");

                    b.HasKey("QuizGroupId");

                    b.HasAlternateKey("Code")
                        .HasName("UQ_QuizGroup_Code");

                    b.HasIndex("QuizCategoryId");

                    b.ToTable("QuizGroups");
                });

            modelBuilder.Entity("QuizMaster.Models.QuizPrerequisites", b =>
                {
                    b.Property<Guid>("QuizId");

                    b.Property<string>("PrerequisiteQuizCode");

                    b.HasKey("QuizId", "PrerequisiteQuizCode");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizPrerequisites");
                });

            modelBuilder.Entity("QuizMaster.Models.QuizQuestion", b =>
                {
                    b.Property<Guid>("QuizId");

                    b.Property<Guid>("QuestionId");

                    b.HasKey("QuizId", "QuestionId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizQuestions");
                });

            modelBuilder.Entity("QuizMaster.Models.QuizSession", b =>
                {
                    b.Property<Guid>("QuizId");

                    b.Property<Guid>("SessionId");

                    b.HasKey("QuizId", "SessionId");

                    b.HasIndex("QuizId");

                    b.HasIndex("SessionId");

                    b.ToTable("QuizSessions");
                });

            modelBuilder.Entity("QuizMaster.Models.Session", b =>
                {
                    b.Property<Guid>("SessionId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ApplicationUserId");

                    b.Property<int>("CorrectAnswerCount");

                    b.Property<DateTime?>("DateCompleted");

                    b.Property<DateTime>("DateTaken");

                    b.Property<int>("QuizItemCount");

                    b.Property<byte>("SessionStatus");

                    b.HasKey("SessionId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("QuizMaster.Models.SessionAnswer", b =>
                {
                    b.Property<Guid>("SessionAnswerId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnswerChronology");

                    b.Property<Guid>("AnswerId");

                    b.Property<bool>("IsCorrect");

                    b.Property<Guid>("SessionId");

                    b.Property<string>("UserAnswer");

                    b.HasKey("SessionAnswerId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("SessionId");

                    b.ToTable("SessionAnswers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("QuizMaster.Models.ApplicationRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("QuizMaster.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("QuizMaster.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("QuizMaster.Models.ApplicationRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QuizMaster.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizMaster.Models.Answer", b =>
                {
                    b.HasOne("QuizMaster.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizMaster.Models.QuestionChoice", b =>
                {
                    b.HasOne("QuizMaster.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizMaster.Models.Quiz", b =>
                {
                    b.HasOne("QuizMaster.Models.QuizGroup", "QuizGroup")
                        .WithMany("Quizes")
                        .HasForeignKey("QuizGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizMaster.Models.QuizChoice", b =>
                {
                    b.HasOne("QuizMaster.Models.Quiz", "Quiz")
                        .WithMany("QuizChoices")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizMaster.Models.QuizGroup", b =>
                {
                    b.HasOne("QuizMaster.Models.QuizCategory", "QuizCategory")
                        .WithMany()
                        .HasForeignKey("QuizCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizMaster.Models.QuizPrerequisites", b =>
                {
                    b.HasOne("QuizMaster.Models.Quiz")
                        .WithMany("QuizPrerequisites")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizMaster.Models.QuizQuestion", b =>
                {
                    b.HasOne("QuizMaster.Models.Question", "Question")
                        .WithMany("QuizQuestions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QuizMaster.Models.Quiz", "Quiz")
                        .WithMany("QuizQuestions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizMaster.Models.QuizSession", b =>
                {
                    b.HasOne("QuizMaster.Models.Quiz", "Quiz")
                        .WithMany("QuizSessions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QuizMaster.Models.Session", "Session")
                        .WithMany("QuizSessions")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizMaster.Models.Session", b =>
                {
                    b.HasOne("QuizMaster.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizMaster.Models.SessionAnswer", b =>
                {
                    b.HasOne("QuizMaster.Models.Answer", "Answer")
                        .WithMany("SessionAnswers")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QuizMaster.Models.Session", "Session")
                        .WithMany("SessionAnswers")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
