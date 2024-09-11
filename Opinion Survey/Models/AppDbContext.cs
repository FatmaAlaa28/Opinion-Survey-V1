using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Opinion_Survey.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }  // Changed get; to get; set;
        public DbSet<Form> Forms { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Responsse> Responsses { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<Folder> Folders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>()
                .HasOne(f => f.ParentFolder)
                .WithMany(f => f.ChildFolders)
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Forms) // Changed from Forms to Form
                .WithMany(f => f.Questions)
                .HasForeignKey(q => q.FormId);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-7TPMJ1D;Initial Catalog=Opinion-Survey;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
