using System;
using System.Collections.Generic;
using DL.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DL.Models
{
    public partial class TicketingSystemContext : DbContext
    {

        private readonly ChangesIntercepor _interceptor;
        public TicketingSystemContext(ChangesIntercepor interceptor)
        {
            _interceptor = interceptor;
        }

        public TicketingSystemContext(DbContextOptions<TicketingSystemContext> options, ChangesIntercepor interceptor)
            : base(options)
        {
            _interceptor = interceptor;
        }

        public virtual DbSet<Call> Calls { get; set; } = null!;
        public virtual DbSet<CallType> CallTypes { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Module> Modules { get; set; } = null!;
        public virtual DbSet<UploadedFile> UploadedFiles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(AppSettings.ConnectionString).EnableSensitiveDataLogging()
                    .ConfigureWarnings(x => x.Ignore(CoreEventId.DetachedLazyLoadingWarning)).AddInterceptors(_interceptor);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Call>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FkAssignedTo).HasColumnName("FK_AssignedTo");

                entity.Property(e => e.FkCallType).HasColumnName("FK_CallType");

                entity.Property(e => e.FkClosedBy).HasColumnName("FK_ClosedBy");

                entity.Property(e => e.FkCreatedBy).HasColumnName("FK_CreatedBy");

                entity.Property(e => e.FkDeletedBy).HasColumnName("FK_DeletedBy");

                entity.Property(e => e.FkModifiedBy).HasColumnName("FK_ModifiedBy");

                entity.Property(e => e.FkModule).HasColumnName("FK_Module");

                entity.Property(e => e.FkRefCall).HasColumnName("FK_RefCall");

                entity.Property(e => e.Subject).HasMaxLength(300);

                entity.Property(e => e.SupportStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.FkAssignedToNavigation)
                    .WithMany(p => p.CallFkAssignedToNavigations)
                    .HasForeignKey(d => d.FkAssignedTo)
                    .HasConstraintName("FK_Calls_Users3");

                entity.HasOne(d => d.FkCallTypeNavigation)
                    .WithMany(p => p.Calls)
                    .HasForeignKey(d => d.FkCallType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Calls_CallTypes");

                entity.HasOne(d => d.FkCreatedByNavigation)
                    .WithMany(p => p.CallFkCreatedByNavigations)
                    .HasForeignKey(d => d.FkCreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Calls_Users");

                entity.HasOne(d => d.FkDeletedByNavigation)
                    .WithMany(p => p.CallFkDeletedByNavigations)
                    .HasForeignKey(d => d.FkDeletedBy)
                    .HasConstraintName("FK_Calls_Users2");

                entity.HasOne(d => d.FkModifiedByNavigation)
                    .WithMany(p => p.CallFkModifiedByNavigations)
                    .HasForeignKey(d => d.FkModifiedBy)
                    .HasConstraintName("FK_Calls_Users1");

                entity.HasOne(d => d.FkModuleNavigation)
                    .WithMany(p => p.Calls)
                    .HasForeignKey(d => d.FkModule)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Calls_Modules");

                entity.HasOne(d => d.FkRefCallNavigation)
                    .WithMany(p => p.InverseFkRefCallNavigation)
                    .HasForeignKey(d => d.FkRefCall)
                    .HasConstraintName("FK_Calls_Calls");
            });

            modelBuilder.Entity<CallType>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.TypeName).HasMaxLength(100);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comment1).HasColumnName("Comment");

                entity.Property(e => e.FkCallId).HasColumnName("FK_CallId");

                entity.Property(e => e.FkCommenterId).HasColumnName("FK_CommenterId");

                entity.HasOne(d => d.FkCall)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.FkCallId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Calls");

                entity.HasOne(d => d.FkCommenter)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.FkCommenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Users");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ModuleName).HasMaxLength(100);
            });

            modelBuilder.Entity<UploadedFile>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FkCallId).HasColumnName("FK_CallID");

                entity.HasOne(d => d.FkCall)
                    .WithMany(p => p.UploadedFiles)
                    .HasForeignKey(d => d.FkCallId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UploadedFiles_Calls");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Password).HasMaxLength(300);

                entity.Property(e => e.UserName).HasMaxLength(300);

                entity.Property(e => e.UserType).HasDefaultValueSql("((1))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
