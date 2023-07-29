﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entities.SeedDataModels;

public partial class LoopHabitsBackupContext : DbContext
{
    public LoopHabitsBackupContext()
    {
    }

    public LoopHabitsBackupContext(DbContextOptions<LoopHabitsBackupContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AndroidMetadatum> AndroidMetadata { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Habit> Habits { get; set; }

    public virtual DbSet<Repetition> Repetitions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=LoopHabitsBackup.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AndroidMetadatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("android_metadata");

            entity.Property(e => e.Locale).HasColumnName("locale");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.ServerId).HasColumnName("server_id");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
        });

        modelBuilder.Entity<Habit>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Archived).HasColumnName("archived");
            entity.Property(e => e.Color).HasColumnName("color");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FreqDen).HasColumnName("freq_den");
            entity.Property(e => e.FreqNum).HasColumnName("freq_num");
            entity.Property(e => e.Highlight).HasColumnName("highlight");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Question).HasColumnName("question");
            entity.Property(e => e.ReminderDays)
                .HasDefaultValueSql("127")
                .HasColumnName("reminder_days");
            entity.Property(e => e.ReminderHour).HasColumnName("reminder_hour");
            entity.Property(e => e.ReminderMin).HasColumnName("reminder_min");
            entity.Property(e => e.TargetType).HasColumnName("target_type");
            entity.Property(e => e.TargetValue).HasColumnName("target_value");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Unit)
                .HasDefaultValueSql("\"\"")
                .HasColumnName("unit");
            entity.Property(e => e.Uuid).HasColumnName("uuid");
        });

        modelBuilder.Entity<Repetition>(entity =>
        {
            entity.HasIndex(e => new { e.Habit, e.Timestamp }, "idx_repetitions_habit_timestamp").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Habit).HasColumnName("habit");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.HabitNavigation).WithMany(p => p.Repetitions)
                .HasForeignKey(d => d.Habit)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
