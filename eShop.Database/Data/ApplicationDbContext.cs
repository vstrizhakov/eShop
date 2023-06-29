using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace eShop.Database.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<CompositionImage> ProductCompilationsImage { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        public DbSet<TelegramUser> TelegramUsers { get; set; }
        public DbSet<TelegramChat> TelegramChats { get; set; }
        public DbSet<TelegramChatMember> TelegramChatMembers { get; set; }
        public DbSet<TelegramChatSettings> TelegramChatSettings { get; set; }

        public DbSet<ViberUser> ViberUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasOne(e => e.ParentCategory)
                .WithMany(e => e.SubCategories)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(e => e.Provider)
                .WithMany(e => e.Clients)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
