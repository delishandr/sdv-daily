using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SDVDaily.Models
{
    public partial class DB_SDV_DailyContext : DbContext
    {
        public DB_SDV_DailyContext()
        {
        }

        public DB_SDV_DailyContext(DbContextOptions<DB_SDV_DailyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Crop> Crops { get; set; } = null!;
        public virtual DbSet<CropCategory> CropCategories { get; set; } = null!;
        public virtual DbSet<CropSeason> CropSeasons { get; set; } = null!;
        public virtual DbSet<Day> Days { get; set; } = null!;
        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<EventDay> EventDays { get; set; } = null!;
        public virtual DbSet<GrowingCrop> GrowingCrops { get; set; } = null!;
        public virtual DbSet<Reminder> Reminders { get; set; } = null!;
        public virtual DbSet<ReminderRepeat> ReminderRepeats { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<SaveFile> SaveFiles { get; set; } = null!;
        public virtual DbSet<Season> Seasons { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Villager> Villagers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=DB_SDV_Daily;User id=sa;Password=P@ssw0rd;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Crop>(entity =>
            {
                entity.ToTable("crop");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GrowthTime).HasColumnName("growthTime");

                entity.Property(e => e.Img)
                    .HasColumnType("text")
                    .HasColumnName("img");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.IsWalkable)
                    .IsRequired()
                    .HasColumnName("isWalkable")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.RegrowthTime).HasColumnName("regrowthTime");

                entity.Property(e => e.SellPrice).HasColumnName("sellPrice");

                entity.Property(e => e.StartYear)
                    .HasColumnName("startYear")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Unirrigated).HasColumnName("unirrigated");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<CropCategory>(entity =>
            {
                entity.ToTable("crop_category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<CropSeason>(entity =>
            {
                entity.ToTable("crop_season");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CropId).HasColumnName("cropId");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.SeasonId).HasColumnName("seasonId");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<Day>(entity =>
            {
                entity.ToTable("day");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("event");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndTime)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("endTime");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Location)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("location");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Preparation)
                    .HasColumnType("text")
                    .HasColumnName("preparation");

                entity.Property(e => e.StartTime)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("startTime");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<EventDay>(entity =>
            {
                entity.ToTable("event_day");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Day).HasColumnName("day");

                entity.Property(e => e.EventId).HasColumnName("eventId");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Season).HasColumnName("season");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<GrowingCrop>(entity =>
            {
                entity.ToTable("growing_crop");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CropId).HasColumnName("cropId");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.IsIndoors).HasColumnName("isIndoors");

                entity.Property(e => e.IsOnGinger).HasColumnName("isOnGinger");

                entity.Property(e => e.NextHarvest).HasColumnName("nextHarvest");

                entity.Property(e => e.NextHarvestSeason).HasColumnName("nextHarvestSeason");

                entity.Property(e => e.SaveId).HasColumnName("saveId");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<Reminder>(entity =>
            {
                entity.ToTable("reminder");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.NextRemind).HasColumnName("nextRemind");

                entity.Property(e => e.NextRemindSeason).HasColumnName("nextRemindSeason");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<ReminderRepeat>(entity =>
            {
                entity.ToTable("reminder_repeat");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Day).HasColumnName("day");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.ReminderId).HasColumnName("reminder_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<SaveFile>(entity =>
            {
                entity.ToTable("save_file");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Day).HasColumnName("day");

                entity.Property(e => e.HasFarmAnimals).HasColumnName("hasFarmAnimals");

                entity.Property(e => e.HasPet).HasColumnName("hasPet");

                entity.Property(e => e.IsAgriculturist).HasColumnName("isAgriculturist");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Season).HasColumnName("season");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.ToTable("season");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("lastLogin");

                entity.Property(e => e.LastSave).HasColumnName("lastSave");

                entity.Property(e => e.LoginAttempt).HasColumnName("loginAttempt");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Villager>(entity =>
            {
                entity.ToTable("villager");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BirthDay).HasColumnName("birthDay");

                entity.Property(e => e.BirthMonth).HasColumnName("birthMonth");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.LovedGifts)
                    .HasColumnType("text")
                    .HasColumnName("lovedGifts");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
