using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Entities
{
    public class BookShopContext:DbContext
    {
        public BookShopContext(DbContextOptions<BookShopContext> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<BookCategories> BookCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x =>x.Id);
            modelBuilder.Entity<User>().Property(x => x.FirstName).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<User>().Property(x => x.LastName).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<User>().Property(x => x.UserName).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<User>().HasIndex(x => x.UserName).IsUnique(true);
            modelBuilder.Entity<User>().HasOne(x => x.Role);
            modelBuilder.Entity<Category>().HasKey(x => x.Id);
            modelBuilder.Entity<Category>().Property(x => x.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Category>().HasIndex(x=>x.Name).IsUnique(true);
            modelBuilder.Entity<Role>().HasKey(x => x.Id);
            modelBuilder.Entity<Role>().Property(x => x.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Role>().HasIndex(x => x.Name).IsUnique(true);
            modelBuilder.Entity<Book>().HasKey(x => x.Id);
            modelBuilder.Entity<Book>().Property(x => x.Title).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Book>().Property(x=>x.Description).IsRequired().HasMaxLength(2000);
            modelBuilder.Entity<Book>().Property(x => x.Title).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Book>().Property(x => x.Price).IsRequired();
            modelBuilder.Entity<BookCategories>().HasKey(x => x.Id);
            modelBuilder.Entity<BookCategories>().HasOne(x => x.Book).WithMany(x => x.Categories).HasForeignKey(x => x.BookId);
            modelBuilder.Entity<BookCategories>().HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
        }
    }
}
