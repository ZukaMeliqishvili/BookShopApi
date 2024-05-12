using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public DbSet<Order> Orders { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<ShoppingCartItem> shoppingCartItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x =>x.Id);
            modelBuilder.Entity<User>().Property(x => x.FirstName).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<User>().Property(x => x.LastName).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<User>().Property(x => x.UserName).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<User>().Property(x => x.Email).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<User>().Property(x => x.PhoneNumber).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<User>().Property(x => x.Address).IsRequired().HasMaxLength(500);
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
            modelBuilder.Entity<OrderItem>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderItem>().Property(x=>x.Quantity).IsRequired();
            modelBuilder.Entity<OrderItem>().HasOne(x => x.Book).WithMany().HasForeignKey(x => x.BookId);
            modelBuilder.Entity<OrderItem>().HasOne(x => x.Order).WithMany(x => x.OrderItems).HasForeignKey(x => x.OrderId);
            modelBuilder.Entity<ShoppingCartItem>().HasKey(x => x.Id);
            modelBuilder.Entity<ShoppingCartItem>().Property(x=>x.Quantity).IsRequired();
            modelBuilder.Entity<ShoppingCartItem>().HasOne(x => x.Book).WithMany().HasForeignKey(x => x.BookId);
            modelBuilder.Entity<ShoppingCartItem>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().Property(x=>x.Currency).IsRequired();
        }
    }
}
