using Microsoft.EntityFrameworkCore;
using taur_bot_api.Database.Models;
using taur_bot_api.src.Database.Models;

namespace taur_bot_api.Database;

public class AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<User> Users { get; init; }
    public DbSet<Investment> Investments { get; set; }
    public DbSet<InvestmentType> InvestmentType { get; set; }
    public DbSet<ReferralNode> ReferralNodes { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Withdraw> Withdraws { get; set; }
    public DbSet<Operation> Operations { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Wallet>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<Wallet>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Investment>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<Investment>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd(); 

        modelBuilder.Entity<Withdraw>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<Withdraw>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<ReferralNode>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<ReferralNode>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Operation>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<Operation>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<InvestmentType>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<InvestmentType>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Investment>()
            .HasOne<InvestmentType>(i => i.InvestmentType)
            .WithMany(i => i.Investments)
            .HasForeignKey(i => i.InvestmentTypeId);

        modelBuilder.Entity<Wallet>()
            .HasOne<User>(w => w.User)
            .WithMany(w => w.Wallets)
            .HasForeignKey(w => w.UserId);
        
        modelBuilder.Entity<Operation>()
            .HasOne<User>(w => w.User)
            .WithMany(w => w.Operations)
            .HasForeignKey(w => w.UserId);

        modelBuilder.Entity<User>()
            .HasOne<User>(u => u.Referrer)
            .WithMany(u => u.Referrals)
            .HasForeignKey(u => u.ReferrerId);

        modelBuilder.Entity<Investment>()
            .HasOne<User>(i => i.User)
            .WithMany(u => u.Investments)
            .HasForeignKey(i => i.UserId); 

        modelBuilder.Entity<Withdraw>()
            .HasOne<User>(i => i.User)
            .WithMany(u => u.Withdraws)
            .HasForeignKey(i => i.UserId);

        modelBuilder.Entity<ReferralNode>()
            .HasOne<User>(r => r.Referral)
            .WithMany(u => u.ReferralNodes)
            .HasForeignKey(r => r.ReferralId);
        
        modelBuilder.Entity<ReferralNode>()
            .HasOne<User>(r => r.Referrer)
            .WithMany(u => u.ReferrerNodes)
            .HasForeignKey(r => r.ReferrerId);
    }
}