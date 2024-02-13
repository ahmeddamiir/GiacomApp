using Call_Detail_Record_Business_Intelligence.Services;
using CsvHelper;
using Microsoft.EntityFrameworkCore;

namespace Call_Detail_Record_Business_Intelligence.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-G7P19QD;Initial Catalog=Cdr;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cdr>().ToTable("techtest_cdr", "dbo").HasNoKey();
        }
        public DbSet<Cdr> Cdrs { get; set; }
    }
}
