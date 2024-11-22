using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Budgie.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CanHaveCategory = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<string>(type: "text", nullable: true),
                    TransactionTypeId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categories_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetLimits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PeriodType = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLimits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetLimits_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetLimits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    OriginalDescription = table.Column<string>(type: "text", nullable: false),
                    ModifiedDescription = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "Id", "CanHaveCategory", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { "direct-credit", false, new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(7984), "Direct Credit", new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(7984) },
                    { "international-purchase", false, new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(7537), "International Purchase", new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(7538) },
                    { "payment", false, new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(8795), "Payment", new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(8796) },
                    { "purchase", false, new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(7017), "Purchase", new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(7018) },
                    { "refund", false, new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(8406), "Refund", new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(8407) },
                    { "transfer", false, new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(9169), "Transfer", new DateTime(2024, 11, 22, 6, 5, 26, 919, DateTimeKind.Utc).AddTicks(9169) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(5977), "user@example.com", "John", "Doe", "$2a$11$O1CxjWAlJuYj1VnD3HCyWu16ty7S7zYUIehSCZtIvGZI7jAnTz.we", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(5978) },
                    { 2L, new DateTime(2024, 11, 22, 6, 5, 27, 66, DateTimeKind.Utc).AddTicks(2248), "user2@example.com", "Jane", "Smith", "$2a$11$wp3KuGUPTygp0QyFz89k4OAX8FHzQDQbutvIDThHdI2vVBXDhiHcS", new DateTime(2024, 11, 22, 6, 5, 27, 66, DateTimeKind.Utc).AddTicks(2249) },
                    { 3L, new DateTime(2024, 11, 22, 6, 5, 27, 207, DateTimeKind.Utc).AddTicks(1925), "user3@example.com", "Bob", "Johnson", "$2a$11$2YCzhW4TsuZH7rkgd33gZeasbgdTva7Wb5RO0SIfvF98yu700zxLK", new DateTime(2024, 11, 22, 6, 5, 27, 207, DateTimeKind.Utc).AddTicks(1926) }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "TransactionTypeId", "UpdatedAt" },
                values: new object[,]
                {
                    { "direct-credit", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(4298), "Direct Credit", null, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(4298) },
                    { "good-life", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(1852), "Good Life", null, "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(1852) },
                    { "home", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(6752), "Home", null, "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(6753) },
                    { "personal", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(4730), "Personal", null, "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(4730) },
                    { "transfer", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(3805), "Transfer", null, "transfer", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(3806) },
                    { "transport", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(9972), "Transport", null, "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(9973) }
                });

            migrationBuilder.InsertData(
                table: "BudgetLimits",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "PeriodType", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1L, 10000m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(9092), "monthly", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(9092), 1L },
                    { 11L, 5000m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(3371), "monthly", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(3371), 1L },
                    { 12L, 50m, "transport", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(3719), "weekly", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(3720), 1L }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "TransactionTypeId", "UpdatedAt" },
                values: new object[,]
                {
                    { "adult", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(2773), "Adult", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(2773) },
                    { "booze", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(2758), "Booze", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(2758) },
                    { "car-insurance-and-maintenance", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(921), "Car Insurance, Rego & Maintenance", "transport", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(922) },
                    { "car-repayments", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(4715), "Car Repayments", "transport", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(4716) },
                    { "clothing-and-accessories", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(3216), "Clothing & Accessories", "personal", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(3217) },
                    { "cycling", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(3647), "Cycling", "transport", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(3647) },
                    { "education-and-student-loans", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(5243), "Education & Student Loans", "personal", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(5244) },
                    { "events-and-gigs", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(5826), "Events & Gigs", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(5826) },
                    { "family", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(1355), "Children & Family", "personal", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(1356) },
                    { "fitness-and-wellbeing", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(7827), "Fitness & Wellbeing", "personal", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(7828) },
                    { "fuel", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(6332), "Fuel", "transport", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(6333) },
                    { "games-and-software", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(394), "Apps, Games & Software", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(394) },
                    { "gifts-and-charity", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(866), "Gifts & Charity", "personal", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(866) },
                    { "groceries", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(2328), "Groceries", "home", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(2328) },
                    { "hair-and-beauty", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(2803), "Hair & Beauty", "personal", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(2803) },
                    { "health-and-medical", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(5247), "Health & Medical", "personal", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(5248) },
                    { "hobbies", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(8317), "Hobbies", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(8317) },
                    { "holidays-and-travel", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(1309), "Holidays & Travel", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(1309) },
                    { "home-insurance-and-rates", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(4153), "Rates & Insurance", "home", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(4154) },
                    { "home-maintenance-and-improvements", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(8854), "Maintenance & Improvements", "home", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(8855) },
                    { "homeware-and-appliances", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(4143), "Homeware & Appliances", "home", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(4143) },
                    { "internet", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(7160), "Internet", "home", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(7161) },
                    { "investments", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(7250), "Investments", "personal", "direct-credit", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(7251) },
                    { "life-admin", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(9572), "Life Admin", "personal", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(9573) },
                    { "lottery-and-gambling", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(3292), "Lottery & Gambling", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(3292) },
                    { "mobile-phone", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(585), "Mobile Phone", "personal", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(585) },
                    { "news-magazines-and-books", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(1619), "News, Magazines & Books", "personal", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(1619) },
                    { "parking", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(9551), "Parking", "transport", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 920, DateTimeKind.Utc).AddTicks(9552) },
                    { "pets", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(1842), "Pets", "home", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(1842) },
                    { "public-transport", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(2288), "Public Transport", "transport", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(2288) },
                    { "pubs-and-bars", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(5767), "Pubs & Bars", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(5767) },
                    { "rent-and-mortgage", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(6298), "Rent & Mortgage", "home", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(6298) },
                    { "restaurants-and-cafes", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(8008), "Restaurants & Cafes", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(8009) },
                    { "takeaway", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(104), "Takeaway", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(105) },
                    { "taxis-and-share-cars", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(6794), "Taxis & Share Cars", "transport", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(6795) },
                    { "technology", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(3226), "Technology", "personal", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(3226) },
                    { "tobacco-and-vaping", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(1071), "Tobacco & Vaping", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(1072) },
                    { "toll-roads", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(8555), "Tolls", "transport", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(8556) },
                    { "tv-and-music", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(2082), "TV, Music & Streaming", "good-life", "purchase", new DateTime(2024, 11, 22, 6, 5, 26, 922, DateTimeKind.Utc).AddTicks(2082) },
                    { "utilities", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(9123), "Utilities", "home", "payment", new DateTime(2024, 11, 22, 6, 5, 26, 921, DateTimeKind.Utc).AddTicks(9123) }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "Currency", "Date", "ModifiedDescription", "OriginalDescription", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 6L, 3000.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(1158), "AUD", new DateOnly(2024, 8, 10), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(1158), 1L },
                    { 7L, -500.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(2405), "AUD", new DateOnly(2024, 8, 11), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(2406), 1L },
                    { 16L, 3200.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(8688), "AUD", new DateOnly(2024, 8, 9), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(8688), 2L },
                    { 17L, -800.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(9233), "AUD", new DateOnly(2024, 8, 11), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(9234), 2L },
                    { 21L, -23.45m, "transport", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(1201), "AUD", new DateOnly(2024, 7, 29), "Uber Rides", "UBER *RIDES SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(1202), 3L },
                    { 26L, 2900.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(6187), "AUD", new DateOnly(2024, 8, 9), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(6187), 3L },
                    { 27L, -1000.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(6720), "AUD", new DateOnly(2024, 8, 10), null, "TRANSFER TO MORTGAGE ACCOUNT ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(6720), 3L },
                    { 33L, -150.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(1175), "AUD", new DateOnly(2024, 8, 17), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(1175), 1L },
                    { 36L, 600.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(2622), "AUD", new DateOnly(2024, 8, 16), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(2622), 2L },
                    { 37L, -250.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(3107), "AUD", new DateOnly(2024, 8, 17), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(3108), 2L },
                    { 39L, -1200.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(4162), "AUD", new DateOnly(2024, 8, 17), null, "BOQ HOME LOAN PAYMENT ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(4162), 3L },
                    { 40L, 500.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(5043), "AUD", new DateOnly(2024, 8, 19), null, "TRANSFER FROM INVESTMENT ACCOUNT ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(5044), 3L },
                    { 41L, -200.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(5845), "AUD", new DateOnly(2024, 8, 20), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(5846), 3L },
                    { 42L, 1500.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(6621), "AUD", new DateOnly(2024, 8, 21), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(6622), 3L },
                    { 45L, 1000.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(8834), "AUD", new DateOnly(2024, 8, 21), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(8835), 2L },
                    { 49L, -150.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(1321), "AUD", new DateOnly(2024, 8, 25), null, "TRANSFER TO FRIEND ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(1321), 1L },
                    { 51L, -200.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(2382), "AUD", new DateOnly(2024, 8, 24), null, "TRANSFER TO FAMILY ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(2382), 2L },
                    { 52L, -500.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(2909), "AUD", new DateOnly(2024, 8, 25), null, "TRANSFER TO SUPERANNUATION ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(2910), 2L },
                    { 53L, 300.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(3418), "AUD", new DateOnly(2024, 8, 26), null, "TRANSFER FROM SAVINGS ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(3418), 3L },
                    { 55L, 3200.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(4437), "AUD", new DateOnly(2024, 8, 28), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(4437), 1L },
                    { 57L, 500.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(5456), "AUD", new DateOnly(2024, 8, 30), "Bonus", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(5456), 1L },
                    { 59L, 3300.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(6440), "AUD", new DateOnly(2024, 8, 28), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(6440), 2L },
                    { 61L, 700.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(7445), "AUD", new DateOnly(2024, 8, 30), "Performance Bonus", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(7445), 2L },
                    { 63L, 3100.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(8434), "AUD", new DateOnly(2024, 8, 28), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(8435), 3L },
                    { 65L, 400.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(9435), "AUD", new DateOnly(2024, 8, 30), "Overtime Payment", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(9435), 3L },
                    { 67L, 3250.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(459), "AUD", new DateOnly(2024, 9, 1), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(459), 1L },
                    { 69L, 3400.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(1499), "AUD", new DateOnly(2024, 9, 1), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(1499), 2L },
                    { 71L, 3200.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(2501), "AUD", new DateOnly(2024, 9, 1), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(2502), 3L },
                    { 73L, -25.50m, "transport", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(3498), "AUD", new DateOnly(2024, 9, 3), "Uber Rides", "UBER *RIDES BRISBANE ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(3498), 1L },
                    { 75L, 3000.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(4758), "AUD", new DateOnly(2024, 9, 6), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(4759), 1L },
                    { 76L, -600.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(5248), "AUD", new DateOnly(2024, 9, 7), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(5248), 1L },
                    { 81L, -200.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(72), "AUD", new DateOnly(2024, 9, 12), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(73), 1L },
                    { 82L, 3200.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(761), "AUD", new DateOnly(2024, 9, 13), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(762), 1L },
                    { 88L, 3400.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(3889), "AUD", new DateOnly(2024, 9, 6), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(3889), 2L },
                    { 89L, -900.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(4480), "AUD", new DateOnly(2024, 9, 7), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(4480), 2L },
                    { 93L, 800.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(6375), "AUD", new DateOnly(2024, 9, 11), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(6375), 2L },
                    { 94L, -300.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(6808), "AUD", new DateOnly(2024, 9, 12), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(6808), 2L },
                    { 95L, 3300.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(7291), "AUD", new DateOnly(2024, 9, 13), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(7292), 2L },
                    { 99L, -35.00m, "transport", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(9191), "AUD", new DateOnly(2024, 9, 3), "Uber Rides", "UBER *RIDES SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(9191), 3L },
                    { 101L, 3100.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(89), "AUD", new DateOnly(2024, 9, 6), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(90), 3L },
                    { 102L, -1200.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(576), "AUD", new DateOnly(2024, 9, 7), null, "TRANSFER TO MORTGAGE ACCOUNT ;", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(576), 3L },
                    { 107L, -250.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(3838), "AUD", new DateOnly(2024, 9, 12), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(3838), 3L },
                    { 108L, 1700.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(4388), "AUD", new DateOnly(2024, 9, 13), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(4389), 3L },
                    { 110L, 3200.00m, "direct-credit", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(6614), "AUD", new DateOnly(2024, 9, 15), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(6615), 3L },
                    { 111L, -1300.00m, "transfer", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(7313), "AUD", new DateOnly(2024, 9, 16), null, "BOQ HOME LOAN PAYMENT ;", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(7314), 3L }
                });

            migrationBuilder.InsertData(
                table: "BudgetLimits",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "PeriodType", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 2L, 250m, "groceries", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(9599), "weekly", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(9600), 1L },
                    { 3L, 150m, "games-and-software", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(9963), "quarterly", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(9964), 1L },
                    { 4L, 400m, "clothing-and-accessories", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(367), "quarterly", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(368), 1L },
                    { 5L, 100m, "fuel", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(759), "weekly", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(759), 1L },
                    { 6L, 50m, "tv-and-music", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(1164), "monthly", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(1164), 1L },
                    { 7L, 1800m, "technology", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(1553), "annual", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(1553), 1L },
                    { 8L, 3000m, "home-maintenance-and-improvements", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(1965), "annual", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(1965), 1L },
                    { 9L, 120m, "restaurants-and-cafes", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(2518), "weekly", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(2518), 1L },
                    { 10L, 300m, "utilities", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(2968), "monthly", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(2969), 1L },
                    { 13L, 2000m, "holidays-and-travel", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(4157), "annual", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(4158), 1L },
                    { 14L, 300m, "fitness-and-wellbeing", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(4525), "monthly", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(4525), 2L },
                    { 15L, 50m, "mobile-phone", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(4948), "monthly", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(4948), 2L },
                    { 16L, 200m, "booze", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(5277), "monthly", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(5277), 3L },
                    { 17L, 800m, "homeware-and-appliances", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(5687), "annual", new DateTime(2024, 11, 22, 6, 5, 27, 364, DateTimeKind.Utc).AddTicks(5687), 3L }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "Currency", "Date", "ModifiedDescription", "OriginalDescription", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1L, -45.60m, "restaurants-and-cafes", new DateTime(2024, 11, 22, 6, 5, 27, 356, DateTimeKind.Utc).AddTicks(6698), "AUD", new DateOnly(2024, 7, 31), "Uber Eats Delivery", "UBER *EATS HELP.UBER.COM ;", new DateTime(2024, 11, 22, 6, 5, 27, 356, DateTimeKind.Utc).AddTicks(6698), 1L },
                    { 2L, -150.00m, "groceries", new DateTime(2024, 11, 22, 6, 5, 27, 356, DateTimeKind.Utc).AddTicks(7422), "AUD", new DateOnly(2024, 8, 2), "Woolworths Groceries", "WOOLWORTHS 1234 SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 356, DateTimeKind.Utc).AddTicks(7422), 1L },
                    { 3L, -200.00m, "clothing-and-accessories", new DateTime(2024, 11, 22, 6, 5, 27, 356, DateTimeKind.Utc).AddTicks(7976), "AUD", new DateOnly(2024, 8, 4), null, "MYER DEPARTMENT STORE SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 356, DateTimeKind.Utc).AddTicks(7976), 1L },
                    { 4L, -450.00m, "holidays-and-travel", new DateTime(2024, 11, 22, 6, 5, 27, 356, DateTimeKind.Utc).AddTicks(9821), "AUD", new DateOnly(2024, 8, 6), "Flights to Melbourne", "FLIGHT CENTRE MELBOURNE ;", new DateTime(2024, 11, 22, 6, 5, 27, 356, DateTimeKind.Utc).AddTicks(9822), 1L },
                    { 5L, -15.99m, "tv-and-music", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(615), "AUD", new DateOnly(2024, 8, 7), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(615), 1L },
                    { 8L, 200.00m, "gifts-and-charity", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(2976), "AUD", new DateOnly(2024, 8, 12), "Gift from Parents", "TRANSFER FROM PARENTS ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(2976), 1L },
                    { 9L, -75.50m, "home-maintenance-and-improvements", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(3576), "AUD", new DateOnly(2024, 8, 14), null, "BUNNINGS WAREHOUSE BRISBANE ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(3577), 1L },
                    { 10L, -120.00m, "technology", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(4772), "AUD", new DateOnly(2024, 8, 15), null, "AMAZON AU MELBOURNE ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(4773), 1L },
                    { 11L, -60.00m, "fuel", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(5370), "AUD", new DateOnly(2024, 7, 30), null, "SHELL SERVICE STATION PERTH ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(5370), 2L },
                    { 12L, -70.00m, "fuel", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(5810), "AUD", new DateOnly(2024, 8, 1), null, "BP AUSTRALIA PERTH ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(5811), 2L },
                    { 13L, -99.99m, "games-and-software", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(6354), "AUD", new DateOnly(2024, 8, 3), null, "APPLE.COM SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(6355), 2L },
                    { 14L, -250.00m, "groceries", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(6917), "AUD", new DateOnly(2024, 8, 4), null, "WOOLWORTHS 5678 PERTH ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(6917), 2L },
                    { 15L, -15.00m, "parking", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(8155), "AUD", new DateOnly(2024, 8, 5), null, "CITY OF PERTH PARKING ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(8156), 2L },
                    { 18L, 500.00m, "gifts-and-charity", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(9674), "AUD", new DateOnly(2024, 8, 12), "Loan from Sister", "TRANSFER FROM SISTER ;", new DateTime(2024, 11, 22, 6, 5, 27, 357, DateTimeKind.Utc).AddTicks(9675), 2L },
                    { 19L, -120.00m, "fitness-and-wellbeing", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(214), "AUD", new DateOnly(2024, 8, 13), null, "REBEL SPORT BRISBANE ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(214), 2L },
                    { 20L, -45.00m, "mobile-phone", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(699), "AUD", new DateOnly(2024, 8, 15), null, "OPTUS BILL PAYMENT ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(700), 2L },
                    { 22L, -180.00m, "groceries", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(1695), "AUD", new DateOnly(2024, 7, 31), "Groceries", "COLES SUPERMARKET ADELAIDE ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(1695), 3L },
                    { 23L, -500.00m, "technology", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(2271), "AUD", new DateOnly(2024, 8, 2), null, "JB HI-FI MELBOURNE ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(2271), 3L },
                    { 24L, -320.00m, "homeware-and-appliances", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(2832), "AUD", new DateOnly(2024, 8, 5), null, "HARVEY NORMAN PERTH ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(2833), 3L },
                    { 25L, -15.99m, "tv-and-music", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(5529), "AUD", new DateOnly(2024, 8, 7), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(5530), 3L },
                    { 28L, 100.00m, "gifts-and-charity", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(7274), "AUD", new DateOnly(2024, 8, 12), "Gift from Friend", "TRANSFER FROM FRIEND ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(7275), 3L },
                    { 29L, -40.00m, "clothing-and-accessories", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(7800), "AUD", new DateOnly(2024, 8, 13), null, "BIG W SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(7800), 3L },
                    { 30L, -60.00m, "booze", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(9411), "AUD", new DateOnly(2024, 8, 14), null, "DAN MURPHY'S BRISBANE ;", new DateTime(2024, 11, 22, 6, 5, 27, 358, DateTimeKind.Utc).AddTicks(9412), 3L },
                    { 31L, -60.00m, "groceries", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(118), "AUD", new DateOnly(2024, 8, 15), null, "WOOLWORTHS 9999 BRISBANE ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(119), 1L },
                    { 32L, -25.00m, "parking", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(680), "AUD", new DateOnly(2024, 8, 16), null, "CITY OF SYDNEY PARKING ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(681), 1L },
                    { 34L, -70.00m, "utilities", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(1643), "AUD", new DateOnly(2024, 8, 18), null, "TELSTRA BILL PAYMENT ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(1643), 1L },
                    { 35L, -200.00m, "clothing-and-accessories", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(2120), "AUD", new DateOnly(2024, 8, 15), null, "WESTFIELD SHOPPING CENTRE PERTH ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(2120), 2L },
                    { 38L, -15.00m, "restaurants-and-cafes", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(3680), "AUD", new DateOnly(2024, 8, 18), null, "SUBWAY FRANCHISE ADELAIDE ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(3681), 2L },
                    { 43L, -900.00m, "rent-and-mortgage", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(7535), "AUD", new DateOnly(2024, 8, 22), null, "AUSSIE HOME LOANS SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(7536), 1L },
                    { 44L, -8.00m, "restaurants-and-cafes", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(8043), "AUD", new DateOnly(2024, 8, 23), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(8043), 1L },
                    { 46L, -60.00m, "home-maintenance-and-improvements", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(9814), "AUD", new DateOnly(2024, 8, 22), null, "BUNNINGS WAREHOUSE PERTH ;", new DateTime(2024, 11, 22, 6, 5, 27, 359, DateTimeKind.Utc).AddTicks(9815), 2L },
                    { 47L, -120.00m, "groceries", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(319), "AUD", new DateOnly(2024, 8, 23), "Groceries", "COLES SUPERMARKET MELBOURNE ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(319), 3L },
                    { 48L, -250.00m, "technology", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(853), "AUD", new DateOnly(2024, 8, 24), null, "KOGAN.COM SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(854), 3L },
                    { 50L, -60.00m, "restaurants-and-cafes", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(1812), "AUD", new DateOnly(2024, 8, 26), "Tickets", "MELBOURNE ZOO MELBOURNE ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(1813), 1L },
                    { 54L, -100.00m, "health-and-medical", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(3967), "AUD", new DateOnly(2024, 8, 27), null, "MEDIBANK PRIVATE SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(3967), 3L },
                    { 56L, 150.00m, "gifts-and-charity", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(4953), "AUD", new DateOnly(2024, 8, 29), "Returned Item Refund", "REFUND FROM AMAZON ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(4954), 1L },
                    { 58L, 20.00m, "groceries", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(5939), "AUD", new DateOnly(2024, 8, 31), "Groceries Refund", "REFUND FROM WOOLWORTHS ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(5939), 1L },
                    { 60L, 10.00m, "games-and-software", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(6939), "AUD", new DateOnly(2024, 8, 29), "App Refund", "REFUND FROM APPLE ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(6940), 2L },
                    { 62L, 30.00m, "fuel", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(7860), "AUD", new DateOnly(2024, 8, 31), "Fuel Refund", "REFUND FROM BP ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(7860), 2L },
                    { 64L, 50.00m, "clothing-and-accessories", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(8915), "AUD", new DateOnly(2024, 8, 29), "Clothing Refund", "REFUND FROM BIG W ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(8915), 3L },
                    { 66L, 100.00m, "technology", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(9934), "AUD", new DateOnly(2024, 8, 31), "Technology Refund", "REFUND FROM JB HI-FI ;", new DateTime(2024, 11, 22, 6, 5, 27, 360, DateTimeKind.Utc).AddTicks(9935), 3L },
                    { 68L, 15.99m, "tv-and-music", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(948), "AUD", new DateOnly(2024, 9, 2), "Subscription Refund", "REFUND FROM NETFLIX ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(948), 1L },
                    { 70L, 25.00m, "fuel", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(1926), "AUD", new DateOnly(2024, 9, 2), "Fuel Refund", "REFUND FROM SHELL ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(1927), 2L },
                    { 72L, 15.00m, "groceries", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(2960), "AUD", new DateOnly(2024, 9, 2), "Groceries Refund", "REFUND FROM COLES ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(2961), 3L },
                    { 74L, -180.00m, "groceries", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(4219), "AUD", new DateOnly(2024, 9, 4), "Woolworths Groceries", "WOOLWORTHS 1234 SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(4219), 1L },
                    { 77L, -220.00m, "technology", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(6444), "AUD", new DateOnly(2024, 9, 8), null, "AMAZON AU SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(6444), 1L },
                    { 78L, -90.00m, "home-maintenance-and-improvements", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(7392), "AUD", new DateOnly(2024, 9, 9), null, "BUNNINGS WAREHOUSE BRISBANE ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(7392), 1L },
                    { 79L, -9.50m, "restaurants-and-cafes", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(7969), "AUD", new DateOnly(2024, 9, 10), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(7969), 1L },
                    { 80L, -85.00m, "utilities", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(8442), "AUD", new DateOnly(2024, 9, 11), null, "TELSTRA BILL PAYMENT ;", new DateTime(2024, 11, 22, 6, 5, 27, 361, DateTimeKind.Utc).AddTicks(8443), 1L },
                    { 83L, -300.00m, "clothing-and-accessories", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(1351), "AUD", new DateOnly(2024, 9, 14), null, "MYER DEPARTMENT STORE SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(1351), 1L },
                    { 84L, -55.00m, "restaurants-and-cafes", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(1822), "AUD", new DateOnly(2024, 9, 15), null, "UBER *EATS SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(1822), 1L },
                    { 85L, -480.00m, "holidays-and-travel", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(2329), "AUD", new DateOnly(2024, 9, 16), "Flights to Sydney", "FLIGHT CENTRE SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(2330), 1L },
                    { 86L, -80.00m, "fuel", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(2904), "AUD", new DateOnly(2024, 9, 3), null, "BP SERVICE STATION PERTH ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(2904), 2L },
                    { 87L, -220.00m, "groceries", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(3429), "AUD", new DateOnly(2024, 9, 4), null, "WOOLWORTHS 5678 PERTH ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(3429), 2L },
                    { 90L, -99.99m, "games-and-software", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(4975), "AUD", new DateOnly(2024, 9, 8), null, "APPLE.COM SYDNEY ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(4975), 2L },
                    { 91L, -130.00m, "fitness-and-wellbeing", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(5423), "AUD", new DateOnly(2024, 9, 9), null, "REBEL SPORT BRISBANE ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(5423), 2L },
                    { 92L, -50.00m, "mobile-phone", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(5898), "AUD", new DateOnly(2024, 9, 10), null, "OPTUS BILL PAYMENT ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(5899), 2L },
                    { 96L, -75.00m, "home-maintenance-and-improvements", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(7789), "AUD", new DateOnly(2024, 9, 14), null, "BUNNINGS WAREHOUSE PERTH ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(7789), 2L },
                    { 97L, -20.00m, "restaurants-and-cafes", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(8259), "AUD", new DateOnly(2024, 9, 15), null, "SUBWAY FRANCHISE ADELAIDE ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(8259), 2L },
                    { 98L, -250.00m, "clothing-and-accessories", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(8729), "AUD", new DateOnly(2024, 9, 16), null, "WESTFIELD SHOPPING CENTRE PERTH ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(8729), 2L },
                    { 100L, -190.00m, "groceries", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(9656), "AUD", new DateOnly(2024, 9, 4), "Groceries", "COLES SUPERMARKET ADELAIDE ;", new DateTime(2024, 11, 22, 6, 5, 27, 362, DateTimeKind.Utc).AddTicks(9657), 3L },
                    { 103L, -550.00m, "technology", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(1773), "AUD", new DateOnly(2024, 9, 8), null, "JB HI-FI MELBOURNE ;", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(1773), 3L },
                    { 104L, -80.00m, "booze", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(2433), "AUD", new DateOnly(2024, 9, 9), null, "DAN MURPHY'S BRISBANE ;", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(2433), 3L },
                    { 105L, -15.99m, "tv-and-music", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(2866), "AUD", new DateOnly(2024, 9, 10), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(2866), 3L },
                    { 106L, -350.00m, "homeware-and-appliances", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(3371), "AUD", new DateOnly(2024, 9, 11), null, "HARVEY NORMAN PERTH ;", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(3371), 3L },
                    { 109L, -10.00m, "restaurants-and-cafes", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(4860), "AUD", new DateOnly(2024, 9, 14), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 11, 22, 6, 5, 27, 363, DateTimeKind.Utc).AddTicks(4860), 3L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLimits_CategoryId",
                table: "BudgetLimits",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLimits_UserId_CategoryId_PeriodType",
                table: "BudgetLimits",
                columns: new[] { "UserId", "CategoryId", "PeriodType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TransactionTypeId",
                table: "Categories",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId_Date_OriginalDescription_Amount",
                table: "Transactions",
                columns: new[] { "UserId", "Date", "OriginalDescription", "Amount" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetLimits");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "TransactionTypes");
        }
    }
}
