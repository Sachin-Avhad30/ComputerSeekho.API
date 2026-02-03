using ComputerSeekho.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComputerSeekho.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
