using eCommerceApp.Entities.Configuration;
using eCommerceApp.Entities.Models;
using eCommerceApp.Entities.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Entities
{
    public class RepositoryDataContext : IdentityDbContext<User>
    {
        public RepositoryDataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());

            //builder.Entity<Tag>().Property(x => x.InsertedAt).HasDefaultValueSql

            // Category n - n Product referencing
            builder.Entity<ProductCategory>().HasKey(x => new { x.CategoryId, x.ProductId });
            builder.Entity<ProductCategory>()
                   .HasOne(x => x.Category)
                   .WithMany(x => x.ProductCategories)
                   .HasForeignKey(x => x.CategoryId);
            builder.Entity<ProductCategory>()
                   .HasOne(x => x.Product)
                   .WithMany(x => x.ProductCategories)
                   .HasForeignKey(x => x.ProductId);

            // Tag n - n Product referencing
            builder.Entity<ProductTag>().HasKey(x => new { x.ProductId, x.TagId });
            builder.Entity<ProductTag>()
                   .HasOne(x => x.Product)
                   .WithMany(x => x.ProductTags)
                   .HasForeignKey(x => x.ProductId);
            builder.Entity<ProductTag>()
                   .HasOne(x => x.Tag)
                   .WithMany(x => x.ProductTags)
                   .HasForeignKey(x => x.TagId);

            // 1 - n self referencing
            builder.Entity<Category>().HasKey(x => x.Id);
            builder.Entity<Category>()
                   .HasOne(x => x.ParentCategory)
                   .WithMany(x => x.ParentCategories)
                   .HasForeignKey(x => x.ParentCategoryId).Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            // Shipment n - n OrderItem referencing
            builder.Entity<ShipmentOrderItem>().HasKey(x => new { x.OrderItemId, x.ShipmentId });
            builder.Entity<ShipmentOrderItem>()
                   .HasOne(x => x.OrderItem)
                   .WithMany(x => x.ShipmentOrderItems)
                   .HasForeignKey(x => x.OrderItemId);
            builder.Entity<ShipmentOrderItem>()
                   .HasOne(x => x.Shipment)
                   .WithMany(x => x.ShipmentOrderItems)
                   .HasForeignKey(x => x.ShipmentId);


            // Atribute n - n ProductAttributeValue referencing
            builder.Entity<AttributeValue>().HasKey(x => new { x.AttributeId, x.ProductAttributeValueId });
            builder.Entity<AttributeValue>()
                   .HasOne(x => x.Attribute)
                   .WithMany(x => x.AttributeValues)
                   .HasForeignKey(x => x.AttributeId);
            builder.Entity<AttributeValue>()
                   .HasOne(x => x.ProductAttributeValue)
                   .WithMany(x => x.AttributeValues)
                   .HasForeignKey(x => x.ProductAttributeValueId);

            //Category n - n AttributeValue referencing
            builder.Entity<CategoryAttributeValue>().HasKey(x => new { x.CategoryId, x.AttributeValueId });
            builder.Entity<CategoryAttributeValue>()
                   .HasOne(x => x.Category)
                   .WithMany(x => x.CategoryAttributeValues)
                   .HasForeignKey(x => x.CategoryId);
            builder.Entity<CategoryAttributeValue>()
                   .HasOne(x => x.AttributeValue)
                   .WithMany(x => x.CategoryAttributeValues)
                   .HasForeignKey(x => x.AttributeValueId)
                   .HasPrincipalKey(x => x.PrincipalKey);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<ProductMedia> ProductMedias { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<CategoryAttributeValue> CategoryAttributeValues { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
    }
}