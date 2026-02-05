using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Enums;
using Microsoft.EntityFrameworkCore;

namespace ComputerSeekho.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

       
        public DbSet<Course> Courses { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Recruiter> RecruiterMasters { get; set; }
        public DbSet<Placement> PlacementMasters { get; set; }
        public DbSet<Student> StudentMasters { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Image> Images { get; set; }

        
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Receipt> Receipts { get; set; }

        // DbSets
        public DbSet<Staff> Staff { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Staff Entity Configuration
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(e => e.StaffId);

                entity.HasIndex(e => e.StaffUsername)
                    .IsUnique();

                entity.HasIndex(e => e.StaffEmail)
                    .IsUnique();

            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId);

                entity.Property(e => e.CourseName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.CourseFees)
                      .IsRequired()
                      .HasColumnType("decimal(10,2)");

                entity.Property(e => e.CourseIsActive)
                      .HasDefaultValue(true);
            });

            // Configure Batch entity
            modelBuilder.Entity<Batch>(entity =>
            {
                entity.HasKey(e => e.BatchId);

                entity.Property(e => e.BatchName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CourseFees)
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.BatchIsActive)
                    .HasDefaultValue(true);

                // Configure relationship with Course
                entity.HasOne(b => b.Course)
                    .WithMany()
                    .HasForeignKey(b => b.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Closure reason
            modelBuilder.Entity<ClosureReason>(entity =>
            {
                entity.HasKey(e => e.ClosureReasonId);

                entity.Property(e => e.ClosureReasonDesc)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true);
            });

            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.HasKey(e => e.AnnouncementId);

                entity.Property(e => e.AnnouncementText)
                    .IsRequired()
                    .HasColumnType("TEXT");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Indexes for better query performance
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => new { e.ValidFrom, e.ValidTo });
            });

            modelBuilder.Entity<Enquiry>(entity =>
            {
                entity.HasKey(e => e.EnquiryId);

                entity.Property(e => e.EnquirerName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EnquirerMobile)
                    .IsRequired();

                entity.Property(e => e.EnquiryProcessedFlag)
                    .HasDefaultValue(false);

                entity.Property(e => e.InquiryCounter)
                    .HasDefaultValue(0);

                entity.Property(e => e.IsClosed)
                    .HasDefaultValue(false);

                // Configure relationship with Course
                entity.HasOne(e => e.Course)
                    .WithMany()
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with Staff
                entity.HasOne(e => e.Staff)
                    .WithMany()
                    .HasForeignKey(e => e.StaffId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with ClosureReason
                entity.HasOne(e => e.ClosureReason)
                    .WithMany()
                    .HasForeignKey(e => e.ClosureReasonId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes for better query performance
                entity.HasIndex(e => e.IsClosed);
                entity.HasIndex(e => e.FollowupDate);
                entity.HasIndex(e => new { e.StaffId, e.IsClosed, e.FollowupDate });
            });

            // ============================================
            // NEW CONFIGURATIONS for Payment System
            // ============================================

            // Configure StudentMaster for Registration Status
            // NOTE: Check if your StudentMaster entity has RegistrationStatus property
            // If not, you can skip this or add the property to StudentMaster
            modelBuilder.Entity<Student>(entity =>
            {
                // Only add this if you've added RegistrationStatus property to StudentMaster
                // entity.Property(s => s.RegistrationStatus)
                //     .HasConversion<int>()
                //     .HasDefaultValue(RegistrationStatus.PaymentPending);
            });

            // Configure PaymentType entity
            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.HasKey(e => e.PaymentTypeId);

                entity.Property(e => e.PaymentTypeDesc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Seed data for payment types
                entity.HasData(
                    new PaymentType
                    {
                        PaymentTypeId = 1,
                        PaymentTypeDesc = "Cash",
                        IsActive = true,
                        CreatedAt = new DateTime(2024, 1, 1)
                    },
                    new PaymentType
                    {
                        PaymentTypeId = 2,
                        PaymentTypeDesc = "Cheque",
                        IsActive = true,
                        CreatedAt = new DateTime(2024, 1, 1)
                    },
                    new PaymentType
                    {
                        PaymentTypeId = 3,
                        PaymentTypeDesc = "Demand Draft (DD)",
                        IsActive = true,
                        CreatedAt = new DateTime(2024, 1, 1)
                    },
                    new PaymentType
                    {
                        PaymentTypeId = 4,
                        PaymentTypeDesc = "Bank Transfer (NEFT/RTGS)",
                        IsActive = true,
                        CreatedAt = new DateTime(2024, 1, 1)
                    },
                    new PaymentType
                    {
                        PaymentTypeId = 5,
                        PaymentTypeDesc = "UPI",
                        IsActive = true,
                        CreatedAt = new DateTime(2024, 1, 1)
                    },
                    new PaymentType
                    {
                        PaymentTypeId = 6,
                        PaymentTypeDesc = "Credit Card",
                        IsActive = true,
                        CreatedAt = new DateTime(2024, 1, 1)
                    },
                    new PaymentType
                    {
                        PaymentTypeId = 7,
                        PaymentTypeDesc = "Debit Card",
                        IsActive = true,
                        CreatedAt = new DateTime(2024, 1, 1)
                    },
                    new PaymentType
                    {
                        PaymentTypeId = 8,
                        PaymentTypeDesc = "Net Banking",
                        IsActive = true,
                        CreatedAt = new DateTime(2024, 1, 1)
                    }
                );
            });

            // Configure Payment entity
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.Property(e => e.PaymentAmount)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PaymentStatus)
                    .HasMaxLength(20)
                    .HasDefaultValue("COMPLETED");

                entity.Property(e => e.TransactionReference)
                    .HasMaxLength(100);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(500);

                // Configure relationships
                entity.HasOne(p => p.Student)
                    .WithMany(s => s.Payments)
                    .HasForeignKey(p => p.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Batch)
                    .WithMany(b => b.Payments)
                    .HasForeignKey(p => p.BatchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.PaymentType)
                    .WithMany(pt => pt.Payments)
                    .HasForeignKey(p => p.PaymentTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes for performance
                entity.HasIndex(e => e.PaymentDate);
                entity.HasIndex(e => e.PaymentStatus);
                entity.HasIndex(e => new { e.StudentId, e.BatchId });
            });

            // Configure Receipt entity
            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.HasKey(e => e.ReceiptId);

                entity.Property(e => e.ReceiptAmount)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(r => r.Payment)
                    .WithMany(p => p.Receipts)
                    .HasForeignKey(r => r.PaymentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.Now;
                }

                entry.Entity.UpdatedAt = DateTime.Now;
            }
        }
    }
}