using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace break_back.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "ingredients",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    is_allergen = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ingredients_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "medical_conditions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("medical_conditions_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "restaurants",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    contact_phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("restaurants_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'Cliente'::character varying"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "meals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    restaurant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("meals_pkey", x => x.id);
                    table.ForeignKey(
                        name: "meals_restaurant_id_fkey",
                        column: x => x.restaurant_id,
                        principalTable: "restaurants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "health_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    goal = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    daily_calorie_target = table.Column<int>(type: "integer", nullable: true),
                    daily_sodium_limit_mg = table.Column<int>(type: "integer", nullable: true),
                    daily_sugar_limit_g = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("health_profiles_pkey", x => x.id);
                    table.ForeignKey(
                        name: "health_profiles_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    restaurant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    order_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'Pendiente'::character varying"),
                    delivery_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("orders_pkey", x => x.id);
                    table.ForeignKey(
                        name: "orders_restaurant_id_fkey",
                        column: x => x.restaurant_id,
                        principalTable: "restaurants",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "orders_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_medical_conditions",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    condition_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_medical_conditions_pkey", x => new { x.user_id, x.condition_id });
                    table.ForeignKey(
                        name: "user_medical_conditions_condition_id_fkey",
                        column: x => x.condition_id,
                        principalTable: "medical_conditions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "user_medical_conditions_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meal_ingredients",
                columns: table => new
                {
                    meal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ingredient_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("meal_ingredients_pkey", x => new { x.meal_id, x.ingredient_id });
                    table.ForeignKey(
                        name: "meal_ingredients_ingredient_id_fkey",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "meal_ingredients_meal_id_fkey",
                        column: x => x.meal_id,
                        principalTable: "meals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nutritional_infos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    meal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    calories = table.Column<int>(type: "integer", nullable: false),
                    protein_g = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    carbs_g = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    fats_g = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    sodium_mg = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    sugar_g = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    fiber_g = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nutritional_infos_pkey", x => x.id);
                    table.ForeignKey(
                        name: "nutritional_infos_meal_id_fkey",
                        column: x => x.meal_id,
                        principalTable: "meals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    meal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("order_items_pkey", x => x.id);
                    table.ForeignKey(
                        name: "order_items_meal_id_fkey",
                        column: x => x.meal_id,
                        principalTable: "meals",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "order_items_order_id_fkey",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "health_profiles_user_id_key",
                table: "health_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ingredients_name_key",
                table: "ingredients",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_meal_ingredients_ingredient_id",
                table: "meal_ingredients",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_meals_restaurant_id",
                table: "meals",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "medical_conditions_name_key",
                table: "medical_conditions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "nutritional_infos_meal_id_key",
                table: "nutritional_infos",
                column: "meal_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_items_meal_id",
                table: "order_items",
                column: "meal_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_order_id",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_restaurant_id",
                table: "orders",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_user_id",
                table: "orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_medical_conditions_condition_id",
                table: "user_medical_conditions",
                column: "condition_id");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "health_profiles");

            migrationBuilder.DropTable(
                name: "meal_ingredients");

            migrationBuilder.DropTable(
                name: "nutritional_infos");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "user_medical_conditions");

            migrationBuilder.DropTable(
                name: "ingredients");

            migrationBuilder.DropTable(
                name: "meals");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "medical_conditions");

            migrationBuilder.DropTable(
                name: "restaurants");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
