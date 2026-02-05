using ComputerSeekho.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComputerSeekho.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Announcement> Announcement { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Recruiter> RecruiterMasters { get; set; }
        public DbSet<Placement> PlacementMasters { get; set; }
        public DbSet<Student> StudentMasters { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Image> Images { get; set; }

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


            //Closure reason
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
            //var entries = ChangeTracker.Entries()
            //    .Where(e => e.Entity is Course &&
            //               (e.State == EntityState.Added || e.State == EntityState.Modified));

            //foreach (var entry in entries)
            //{
            //    var course = (Course)entry.Entity;

            //    if (entry.State == EntityState.Added)
            //    {
            //        course.CreatedAt = DateTime.Now;
            //        course.UpdatedAt = DateTime.Now;
            //    }
            //    else if (entry.State == EntityState.Modified)
            //    {
            //        course.UpdatedAt = DateTime.Now;

            //        // prevent CreatedAt from being updated
            //        entry.Property(nameof(Course.CreatedAt)).IsModified = false;
            //    }



            //}

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
