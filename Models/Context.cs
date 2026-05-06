using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace break_back.Models;

public partial class Context : DbContext
{
    public Context()
    {
    }

    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public virtual DbSet<HealthProfile> HealthProfiles { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<MedicalCondition> MedicalConditions { get; set; }

    public virtual DbSet<NutritionalInfo> NutritionalInfos { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Esto puede quedar vacío o usarse solo para herramientas de diseño
            // Pero lo ideal es que Program.cs maneje la inyección.
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<HealthProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("health_profiles_pkey");

            entity.ToTable("health_profiles");

            entity.HasIndex(e => e.UserId, "health_profiles_user_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.DailyCalorieTarget).HasColumnName("daily_calorie_target");
            entity.Property(e => e.DailySodiumLimitMg).HasColumnName("daily_sodium_limit_mg");
            entity.Property(e => e.DailySugarLimitG).HasColumnName("daily_sugar_limit_g");
            entity.Property(e => e.Goal)
                .HasMaxLength(100)
                .HasColumnName("goal");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.HealthProfile)
                .HasForeignKey<HealthProfile>(d => d.UserId)
                .HasConstraintName("health_profiles_user_id_fkey");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ingredients_pkey");

            entity.ToTable("ingredients");

            entity.HasIndex(e => e.Name, "ingredients_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsAllergen)
                .HasDefaultValue(false)
                .HasColumnName("is_allergen");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("meals_pkey");

            entity.ToTable("meals");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("image_url");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Meals)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("meals_restaurant_id_fkey");

            entity.HasMany(d => d.Ingredients).WithMany(p => p.Meals)
                .UsingEntity<Dictionary<string, object>>(
                    "MealIngredient",
                    r => r.HasOne<Ingredient>().WithMany()
                        .HasForeignKey("IngredientId")
                        .HasConstraintName("meal_ingredients_ingredient_id_fkey"),
                    l => l.HasOne<Meal>().WithMany()
                        .HasForeignKey("MealId")
                        .HasConstraintName("meal_ingredients_meal_id_fkey"),
                    j =>
                    {
                        j.HasKey("MealId", "IngredientId").HasName("meal_ingredients_pkey");
                        j.ToTable("meal_ingredients");
                        j.IndexerProperty<Guid>("MealId").HasColumnName("meal_id");
                        j.IndexerProperty<int>("IngredientId").HasColumnName("ingredient_id");
                    });
        });

        modelBuilder.Entity<MedicalCondition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("medical_conditions_pkey");

            entity.ToTable("medical_conditions");

            entity.HasIndex(e => e.Name, "medical_conditions_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<NutritionalInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("nutritional_infos_pkey");

            entity.ToTable("nutritional_infos");

            entity.HasIndex(e => e.MealId, "nutritional_infos_meal_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Calories).HasColumnName("calories");
            entity.Property(e => e.CarbsG)
                .HasPrecision(5, 2)
                .HasColumnName("carbs_g");
            entity.Property(e => e.FatsG)
                .HasPrecision(5, 2)
                .HasColumnName("fats_g");
            entity.Property(e => e.FiberG)
                .HasPrecision(5, 2)
                .HasColumnName("fiber_g");
            entity.Property(e => e.MealId).HasColumnName("meal_id");
            entity.Property(e => e.ProteinG)
                .HasPrecision(5, 2)
                .HasColumnName("protein_g");
            entity.Property(e => e.SodiumMg)
                .HasPrecision(6, 2)
                .HasColumnName("sodium_mg");
            entity.Property(e => e.SugarG)
                .HasPrecision(5, 2)
                .HasColumnName("sugar_g");

            entity.HasOne(d => d.Meal).WithOne(p => p.NutritionalInfo)
                .HasForeignKey<NutritionalInfo>(d => d.MealId)
                .HasConstraintName("nutritional_infos_meal_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeliveryAddress)
                .HasMaxLength(500)
                .HasColumnName("delivery_address");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pendiente'::character varying")
                .HasColumnName("order_status");
            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_restaurant_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_user_id_fkey");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_items_pkey");

            entity.ToTable("order_items");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.MealId).HasColumnName("meal_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Meal).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.MealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_items_meal_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_items_order_id_fkey");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("restaurants_pkey");

            entity.ToTable("restaurants");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(50)
                .HasColumnName("contact_phone");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Cliente'::character varying")
                .HasColumnName("role");

            entity.HasMany(d => d.Conditions).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserMedicalCondition",
                    r => r.HasOne<MedicalCondition>().WithMany()
                        .HasForeignKey("ConditionId")
                        .HasConstraintName("user_medical_conditions_condition_id_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("user_medical_conditions_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "ConditionId").HasName("user_medical_conditions_pkey");
                        j.ToTable("user_medical_conditions");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("ConditionId").HasColumnName("condition_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
