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
                    { "credit", false, new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(3107), "Credit", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(3108) },
                    { "expense", false, new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(4256), "Expense", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(4257) },
                    { "none", false, new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(5596), "None", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(5597) },
                    { "transfer", false, new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(5113), "Transfer", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(5113) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(5127), "user@example.com", "John", "Doe", "$2a$11$lNRvjtjDv3itoHA16I82Y.bTDVXgSM24Cgy5SqYH093.Julc1Z7um", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(5127) },
                    { 2L, new DateTime(2024, 12, 3, 11, 55, 41, 66, DateTimeKind.Utc).AddTicks(9090), "user2@example.com", "Jane", "Smith", "$2a$11$enUsjovM.f/NC0ilfWYXy.WRVN0hjiXP8ne2lgv9ZSRzmAXnrXVIC", new DateTime(2024, 12, 3, 11, 55, 41, 66, DateTimeKind.Utc).AddTicks(9090) },
                    { 3L, new DateTime(2024, 12, 3, 11, 55, 41, 257, DateTimeKind.Utc).AddTicks(326), "user3@example.com", "Bob", "Johnson", "$2a$11$cqkBBkOMnCzKj7c/iZrRGOqozw9E0xWeraZUe7U.xF/GYxmmkKXP2", new DateTime(2024, 12, 3, 11, 55, 41, 257, DateTimeKind.Utc).AddTicks(327) }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "TransactionTypeId", "UpdatedAt" },
                values: new object[,]
                {
                    { "direct-credit", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(3240), "Direct Credit", null, "credit", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(3240) },
                    { "good-life", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(8764), "Good Life", null, "expense", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(8765) },
                    { "home", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(4746), "Home", null, "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(4747) },
                    { "none", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(3745), "None", null, "none", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(3746) },
                    { "personal", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(1990), "Personal", null, "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(1990) },
                    { "transfer", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(2696), "Transfer", null, "transfer", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(2696) },
                    { "transport", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(7983), "Transport", null, "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(7984) }
                });

            migrationBuilder.InsertData(
                table: "BudgetLimits",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "PeriodType", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1L, 10000m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(8401), "monthly", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(8402), 1L },
                    { 11L, 5000m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(3284), "monthly", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(3284), 1L },
                    { 12L, 50m, "transport", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(3718), "weekly", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(3718), 1L }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "TransactionTypeId", "UpdatedAt" },
                values: new object[,]
                {
                    { "adult", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(1682), "Adult", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(1683) },
                    { "booze", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(9797), "Booze", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(9797) },
                    { "car-insurance-and-maintenance", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(7743), "Car Insurance, Rego & Maintenance", "transport", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(7744) },
                    { "car-repayments", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(3107), "Car Repayments", "transport", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(3108) },
                    { "clothing-and-accessories", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(484), "Clothing & Accessories", "personal", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(484) },
                    { "cycling", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(954), "Cycling", "transport", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(954) },
                    { "education-and-student-loans", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(2572), "Education & Student Loans", "personal", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(2572) },
                    { "events-and-gigs", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(3385), "Events & Gigs", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(3386) },
                    { "family", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(8289), "Children & Family", "personal", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(8290) },
                    { "fitness-and-wellbeing", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(5881), "Fitness & Wellbeing", "personal", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(5882) },
                    { "fuel", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(4199), "Fuel", "transport", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(4199) },
                    { "games-and-software", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(7142), "Apps, Games & Software", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(7143) },
                    { "gifts-and-charity", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(8451), "Gifts & Charity", "personal", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(8451) },
                    { "groceries", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(9329), "Groceries", "home", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 851, DateTimeKind.Utc).AddTicks(9329) },
                    { "hair-and-beauty", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(928), "Hair & Beauty", "personal", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(929) },
                    { "health-and-medical", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(3781), "Health & Medical", "personal", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(3782) },
                    { "hobbies", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(6418), "Hobbies", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(6419) },
                    { "holidays-and-travel", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(9024), "Holidays & Travel", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(9025) },
                    { "home-insurance-and-rates", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(2040), "Rates & Insurance", "home", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(2041) },
                    { "home-maintenance-and-improvements", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(6945), "Maintenance & Improvements", "home", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(6946) },
                    { "homeware-and-appliances", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(1536), "Homeware & Appliances", "home", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(1536) },
                    { "internet", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(5357), "Internet", "home", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(5357) },
                    { "investments", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(6192), "Investments", "personal", "credit", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(6193) },
                    { "life-admin", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(8300), "Life Admin", "personal", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(8300) },
                    { "lottery-and-gambling", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(1414), "Lottery & Gambling", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(1415) },
                    { "mobile-phone", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(9511), "Mobile Phone", "personal", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(9511) },
                    { "news-magazines-and-books", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(644), "News, Magazines & Books", "personal", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(645) },
                    { "parking", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(7459), "Parking", "transport", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(7459) },
                    { "pets", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(9489), "Pets", "home", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 852, DateTimeKind.Utc).AddTicks(9489) },
                    { "public-transport", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(342), "Public Transport", "transport", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(343) },
                    { "pubs-and-bars", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(4375), "Pubs & Bars", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(4376) },
                    { "rent-and-mortgage", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(4970), "Rent & Mortgage", "home", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(4970) },
                    { "restaurants-and-cafes", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(6705), "Restaurants & Cafes", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(6706) },
                    { "takeaway", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(8862), "Takeaway", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(8863) },
                    { "taxis-and-share-cars", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(5457), "Taxis & Share Cars", "transport", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(5457) },
                    { "technology", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(2177), "Technology", "personal", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(2177) },
                    { "tobacco-and-vaping", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(84), "Tobacco & Vaping", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(84) },
                    { "toll-roads", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(7272), "Tolls", "transport", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(7272) },
                    { "tv-and-music", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(1204), "TV, Music & Streaming", "good-life", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 854, DateTimeKind.Utc).AddTicks(1204) },
                    { "utilities", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(7780), "Utilities", "home", "expense", new DateTime(2024, 12, 3, 11, 55, 40, 853, DateTimeKind.Utc).AddTicks(7780) }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "Currency", "Date", "ModifiedDescription", "OriginalDescription", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 2L, -150.0m, "none", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(209), "AUD", new DateOnly(2024, 10, 19), "Woolworths Groceries", "WOOLWORTHS 1234 SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(209), 1L },
                    { 5L, -15.99m, "none", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(2165), "AUD", new DateOnly(2024, 10, 24), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(2165), 1L },
                    { 6L, 3000.0m, "none", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(2681), "AUD", new DateOnly(2024, 10, 27), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(2682), 1L },
                    { 7L, -500.0m, "none", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(3361), "AUD", new DateOnly(2024, 10, 28), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(3374), 1L },
                    { 16L, 3200.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(553), "AUD", new DateOnly(2024, 10, 26), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(554), 2L },
                    { 17L, -800.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(1140), "AUD", new DateOnly(2024, 10, 28), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(1140), 2L },
                    { 21L, -23.45m, "transport", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(3151), "AUD", new DateOnly(2024, 10, 15), "Uber Rides", "UBER *RIDES SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(3151), 3L },
                    { 26L, 2900.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(5836), "AUD", new DateOnly(2024, 10, 26), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(5837), 3L },
                    { 27L, -1000.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(6410), "AUD", new DateOnly(2024, 10, 27), null, "TRANSFER TO MORTGAGE ACCOUNT ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(6410), 3L },
                    { 33L, -150.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(1027), "AUD", new DateOnly(2024, 11, 3), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(1028), 1L },
                    { 36L, 600.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(2580), "AUD", new DateOnly(2024, 11, 2), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(2580), 2L },
                    { 37L, -250.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(3091), "AUD", new DateOnly(2024, 11, 3), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(3091), 2L },
                    { 39L, -1200.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(4144), "AUD", new DateOnly(2024, 11, 3), null, "BOQ HOME LOAN PAYMENT ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(4144), 3L },
                    { 40L, 500.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(4747), "AUD", new DateOnly(2024, 11, 5), null, "TRANSFER FROM INVESTMENT ACCOUNT ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(4747), 3L },
                    { 41L, -200.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(5812), "AUD", new DateOnly(2024, 11, 6), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(5813), 3L },
                    { 42L, 1500.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(6905), "AUD", new DateOnly(2024, 11, 7), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(6905), 3L },
                    { 45L, 1000.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(9790), "AUD", new DateOnly(2024, 11, 7), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(9790), 2L },
                    { 49L, -150.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(2720), "AUD", new DateOnly(2024, 11, 11), null, "TRANSFER TO FRIEND ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(2720), 1L },
                    { 51L, -200.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(3741), "AUD", new DateOnly(2024, 11, 10), null, "TRANSFER TO FAMILY ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(3742), 2L },
                    { 52L, -500.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(4450), "AUD", new DateOnly(2024, 11, 11), null, "TRANSFER TO SUPERANNUATION ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(4451), 2L },
                    { 53L, 300.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(5031), "AUD", new DateOnly(2024, 11, 12), null, "TRANSFER FROM SAVINGS ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(5031), 3L },
                    { 55L, 3200.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(6094), "AUD", new DateOnly(2024, 11, 14), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(6095), 1L },
                    { 57L, 500.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(7209), "AUD", new DateOnly(2024, 11, 16), "Bonus", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(7209), 1L },
                    { 59L, 3300.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(8228), "AUD", new DateOnly(2024, 11, 14), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(8228), 2L },
                    { 61L, 700.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(9248), "AUD", new DateOnly(2024, 11, 16), "Performance Bonus", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(9249), 2L },
                    { 62L, 30.0m, "none", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(498), "AUD", new DateOnly(2024, 11, 17), "Fuel Refund", "REFUND FROM BP ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(498), 2L },
                    { 63L, 3100.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(1016), "AUD", new DateOnly(2024, 11, 14), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(1017), 3L },
                    { 65L, 400.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(2085), "AUD", new DateOnly(2024, 11, 16), "Overtime Payment", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(2086), 3L },
                    { 67L, 3250.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(3044), "AUD", new DateOnly(2024, 11, 18), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(3044), 1L },
                    { 69L, 3400.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(4028), "AUD", new DateOnly(2024, 11, 18), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(4028), 2L },
                    { 71L, 3200.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(4955), "AUD", new DateOnly(2024, 11, 18), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(4955), 3L },
                    { 73L, -25.5m, "transport", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(5967), "AUD", new DateOnly(2024, 11, 20), "Uber Rides", "UBER *RIDES BRISBANE ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(5967), 1L },
                    { 75L, 3000.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(6985), "AUD", new DateOnly(2024, 11, 23), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(6986), 1L },
                    { 76L, -600.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(7460), "AUD", new DateOnly(2024, 11, 24), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(7461), 1L },
                    { 81L, -200.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(434), "AUD", new DateOnly(2024, 11, 29), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(434), 1L },
                    { 82L, 3200.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(956), "AUD", new DateOnly(2024, 11, 30), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(956), 1L },
                    { 88L, 3400.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(4010), "AUD", new DateOnly(2024, 11, 23), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(4010), 2L },
                    { 89L, -900.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(4757), "AUD", new DateOnly(2024, 11, 24), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(4757), 2L },
                    { 93L, 800.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(7035), "AUD", new DateOnly(2024, 11, 28), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(7035), 2L },
                    { 94L, -300.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(7493), "AUD", new DateOnly(2024, 11, 29), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(7493), 2L },
                    { 95L, 3300.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(8043), "AUD", new DateOnly(2024, 11, 30), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(8043), 2L },
                    { 99L, -35.0m, "transport", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(225), "AUD", new DateOnly(2024, 11, 20), "Uber Rides", "UBER *RIDES SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(225), 3L },
                    { 101L, 3100.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(1186), "AUD", new DateOnly(2024, 11, 23), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(1187), 3L },
                    { 102L, -1200.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(1699), "AUD", new DateOnly(2024, 11, 24), null, "TRANSFER TO MORTGAGE ACCOUNT ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(1699), 3L },
                    { 103L, -550.0m, "none", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(2223), "AUD", new DateOnly(2024, 11, 25), null, "JB HI-FI MELBOURNE ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(2223), 3L },
                    { 107L, -250.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(4254), "AUD", new DateOnly(2024, 11, 29), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(4255), 3L },
                    { 108L, 1700.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(4766), "AUD", new DateOnly(2024, 11, 30), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(4766), 3L },
                    { 110L, 3200.0m, "direct-credit", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(5814), "AUD", new DateOnly(2024, 12, 2), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(5814), 3L },
                    { 111L, -1300.0m, "transfer", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(6365), "AUD", new DateOnly(2024, 12, 3), null, "BOQ HOME LOAN PAYMENT ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(6365), 3L }
                });

            migrationBuilder.InsertData(
                table: "BudgetLimits",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "PeriodType", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 2L, 250m, "groceries", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(8828), "weekly", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(8829), 1L },
                    { 3L, 150m, "games-and-software", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(9455), "quarterly", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(9455), 1L },
                    { 4L, 400m, "clothing-and-accessories", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(9868), "quarterly", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(9868), 1L },
                    { 5L, 100m, "fuel", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(309), "weekly", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(309), 1L },
                    { 6L, 50m, "tv-and-music", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(704), "monthly", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(705), 1L },
                    { 7L, 1800m, "technology", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(1063), "annual", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(1063), 1L },
                    { 8L, 3000m, "home-maintenance-and-improvements", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(1524), "annual", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(1525), 1L },
                    { 9L, 120m, "restaurants-and-cafes", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(1905), "weekly", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(1905), 1L },
                    { 10L, 300m, "utilities", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(2831), "monthly", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(2831), 1L },
                    { 13L, 2000m, "holidays-and-travel", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(4198), "annual", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(4199), 1L },
                    { 14L, 300m, "fitness-and-wellbeing", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(4571), "monthly", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(4572), 2L },
                    { 15L, 50m, "mobile-phone", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(4993), "monthly", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(4993), 2L },
                    { 16L, 200m, "booze", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(5386), "monthly", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(5386), 3L },
                    { 17L, 800m, "homeware-and-appliances", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(5774), "annual", new DateTime(2024, 12, 3, 11, 55, 41, 439, DateTimeKind.Utc).AddTicks(5775), 3L }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "Currency", "Date", "ModifiedDescription", "OriginalDescription", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1L, -45.6m, "restaurants-and-cafes", new DateTime(2024, 12, 3, 11, 55, 41, 431, DateTimeKind.Utc).AddTicks(9313), "AUD", new DateOnly(2024, 10, 17), "Uber Eats Delivery", "UBER *EATS HELP.UBER.COM ;", new DateTime(2024, 12, 3, 11, 55, 41, 431, DateTimeKind.Utc).AddTicks(9313), 1L },
                    { 3L, -200.0m, "clothing-and-accessories", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(1009), "AUD", new DateOnly(2024, 10, 21), null, "MYER DEPARTMENT STORE SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(1009), 1L },
                    { 4L, -450.0m, "holidays-and-travel", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(1577), "AUD", new DateOnly(2024, 10, 23), "Flights to Melbourne", "FLIGHT CENTRE MELBOURNE ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(1578), 1L },
                    { 8L, 200.0m, "gifts-and-charity", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(3860), "AUD", new DateOnly(2024, 10, 29), "Gift from Parents", "TRANSFER FROM PARENTS ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(3860), 1L },
                    { 9L, -75.5m, "home-maintenance-and-improvements", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(4532), "AUD", new DateOnly(2024, 10, 31), null, "BUNNINGS WAREHOUSE BRISBANE ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(4532), 1L },
                    { 10L, -120.0m, "technology", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(5054), "AUD", new DateOnly(2024, 11, 1), null, "AMAZON AU MELBOURNE ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(5054), 1L },
                    { 11L, -60.0m, "fuel", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(5575), "AUD", new DateOnly(2024, 10, 16), null, "SHELL SERVICE STATION PERTH ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(5576), 2L },
                    { 12L, -70.0m, "fuel", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(6059), "AUD", new DateOnly(2024, 10, 18), null, "BP AUSTRALIA PERTH ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(6060), 2L },
                    { 13L, -99.99m, "games-and-software", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(8299), "AUD", new DateOnly(2024, 10, 20), null, "APPLE.COM SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(8300), 2L },
                    { 14L, -250.0m, "groceries", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(9120), "AUD", new DateOnly(2024, 10, 21), null, "WOOLWORTHS 5678 PERTH ;", new DateTime(2024, 12, 3, 11, 55, 41, 432, DateTimeKind.Utc).AddTicks(9120), 2L },
                    { 15L, -15.0m, "parking", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(6), "AUD", new DateOnly(2024, 10, 22), null, "CITY OF PERTH PARKING ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(6), 2L },
                    { 18L, 500.0m, "gifts-and-charity", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(1613), "AUD", new DateOnly(2024, 10, 29), "Loan from Sister", "TRANSFER FROM SISTER ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(1614), 2L },
                    { 19L, -120.0m, "fitness-and-wellbeing", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(2133), "AUD", new DateOnly(2024, 10, 30), null, "REBEL SPORT BRISBANE ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(2133), 2L },
                    { 20L, -45.0m, "mobile-phone", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(2650), "AUD", new DateOnly(2024, 11, 1), null, "OPTUS BILL PAYMENT ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(2651), 2L },
                    { 22L, -180.0m, "groceries", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(3686), "AUD", new DateOnly(2024, 10, 17), "Groceries", "COLES SUPERMARKET ADELAIDE ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(3687), 3L },
                    { 23L, -500.0m, "technology", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(4158), "AUD", new DateOnly(2024, 10, 19), null, "JB HI-FI MELBOURNE ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(4159), 3L },
                    { 24L, -320.0m, "homeware-and-appliances", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(4779), "AUD", new DateOnly(2024, 10, 22), null, "HARVEY NORMAN PERTH ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(4779), 3L },
                    { 25L, -15.99m, "tv-and-music", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(5346), "AUD", new DateOnly(2024, 10, 24), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(5346), 3L },
                    { 28L, 100.0m, "gifts-and-charity", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(6861), "AUD", new DateOnly(2024, 10, 29), "Gift from Friend", "TRANSFER FROM FRIEND ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(6861), 3L },
                    { 29L, -40.0m, "clothing-and-accessories", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(8166), "AUD", new DateOnly(2024, 10, 30), null, "BIG W SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(8166), 3L },
                    { 30L, -60.0m, "booze", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(9196), "AUD", new DateOnly(2024, 10, 31), null, "DAN MURPHY'S BRISBANE ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(9196), 3L },
                    { 31L, -60.0m, "groceries", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(9807), "AUD", new DateOnly(2024, 11, 1), null, "WOOLWORTHS 9999 BRISBANE ;", new DateTime(2024, 12, 3, 11, 55, 41, 433, DateTimeKind.Utc).AddTicks(9807), 1L },
                    { 32L, -25.0m, "parking", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(381), "AUD", new DateOnly(2024, 11, 2), null, "CITY OF SYDNEY PARKING ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(381), 1L },
                    { 34L, -70.0m, "utilities", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(1531), "AUD", new DateOnly(2024, 11, 4), null, "TELSTRA BILL PAYMENT ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(1531), 1L },
                    { 35L, -200.0m, "clothing-and-accessories", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(2064), "AUD", new DateOnly(2024, 11, 1), null, "WESTFIELD SHOPPING CENTRE PERTH ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(2064), 2L },
                    { 38L, -15.0m, "restaurants-and-cafes", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(3607), "AUD", new DateOnly(2024, 11, 4), null, "SUBWAY FRANCHISE ADELAIDE ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(3607), 2L },
                    { 43L, -900.0m, "rent-and-mortgage", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(8118), "AUD", new DateOnly(2024, 11, 8), null, "AUSSIE HOME LOANS SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(8118), 1L },
                    { 44L, -8.0m, "restaurants-and-cafes", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(8931), "AUD", new DateOnly(2024, 11, 9), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 12, 3, 11, 55, 41, 434, DateTimeKind.Utc).AddTicks(8932), 1L },
                    { 46L, -60.0m, "home-maintenance-and-improvements", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(601), "AUD", new DateOnly(2024, 11, 8), null, "BUNNINGS WAREHOUSE PERTH ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(602), 2L },
                    { 47L, -120.0m, "groceries", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(1462), "AUD", new DateOnly(2024, 11, 9), "Groceries", "COLES SUPERMARKET MELBOURNE ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(1463), 3L },
                    { 48L, -250.0m, "technology", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(2141), "AUD", new DateOnly(2024, 11, 10), null, "KOGAN.COM SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(2141), 3L },
                    { 50L, -60.0m, "restaurants-and-cafes", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(3216), "AUD", new DateOnly(2024, 11, 12), "Tickets", "MELBOURNE ZOO MELBOURNE ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(3216), 1L },
                    { 54L, -100.0m, "health-and-medical", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(5576), "AUD", new DateOnly(2024, 11, 13), null, "MEDIBANK PRIVATE SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(5577), 3L },
                    { 56L, 150.0m, "gifts-and-charity", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(6616), "AUD", new DateOnly(2024, 11, 15), "Returned Item Refund", "REFUND FROM AMAZON ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(6617), 1L },
                    { 58L, 20.0m, "groceries", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(7748), "AUD", new DateOnly(2024, 11, 17), "Groceries Refund", "REFUND FROM WOOLWORTHS ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(7748), 1L },
                    { 60L, 10.0m, "games-and-software", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(8766), "AUD", new DateOnly(2024, 11, 15), "App Refund", "REFUND FROM APPLE ;", new DateTime(2024, 12, 3, 11, 55, 41, 435, DateTimeKind.Utc).AddTicks(8766), 2L },
                    { 64L, 50.0m, "clothing-and-accessories", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(1509), "AUD", new DateOnly(2024, 11, 15), "Clothing Refund", "REFUND FROM BIG W ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(1510), 3L },
                    { 66L, 100.0m, "technology", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(2593), "AUD", new DateOnly(2024, 11, 17), "Technology Refund", "REFUND FROM JB HI-FI ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(2594), 3L },
                    { 68L, 15.99m, "tv-and-music", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(3508), "AUD", new DateOnly(2024, 11, 19), "Subscription Refund", "REFUND FROM NETFLIX ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(3508), 1L },
                    { 70L, 25.0m, "fuel", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(4502), "AUD", new DateOnly(2024, 11, 19), "Fuel Refund", "REFUND FROM SHELL ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(4503), 2L },
                    { 72L, 15.0m, "groceries", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(5471), "AUD", new DateOnly(2024, 11, 19), "Groceries Refund", "REFUND FROM COLES ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(5471), 3L },
                    { 74L, -180.0m, "groceries", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(6476), "AUD", new DateOnly(2024, 11, 21), "Woolworths Groceries", "WOOLWORTHS 1234 SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(6476), 1L },
                    { 77L, -220.0m, "technology", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(7921), "AUD", new DateOnly(2024, 11, 25), null, "AMAZON AU SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(7922), 1L },
                    { 78L, -90.0m, "home-maintenance-and-improvements", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(8688), "AUD", new DateOnly(2024, 11, 26), null, "BUNNINGS WAREHOUSE BRISBANE ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(8689), 1L },
                    { 79L, -9.5m, "restaurants-and-cafes", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(9311), "AUD", new DateOnly(2024, 11, 27), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(9311), 1L },
                    { 80L, -85.0m, "utilities", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(9797), "AUD", new DateOnly(2024, 11, 28), null, "TELSTRA BILL PAYMENT ;", new DateTime(2024, 12, 3, 11, 55, 41, 436, DateTimeKind.Utc).AddTicks(9798), 1L },
                    { 83L, -300.0m, "clothing-and-accessories", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(1432), "AUD", new DateOnly(2024, 12, 1), null, "MYER DEPARTMENT STORE SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(1432), 1L },
                    { 84L, -55.0m, "restaurants-and-cafes", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(1932), "AUD", new DateOnly(2024, 12, 2), null, "UBER *EATS SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(1932), 1L },
                    { 85L, -480.0m, "holidays-and-travel", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(2479), "AUD", new DateOnly(2024, 12, 3), "Flights to Sydney", "FLIGHT CENTRE SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(2479), 1L },
                    { 86L, -80.0m, "fuel", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(2977), "AUD", new DateOnly(2024, 11, 20), null, "BP SERVICE STATION PERTH ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(2977), 2L },
                    { 87L, -220.0m, "groceries", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(3480), "AUD", new DateOnly(2024, 11, 21), null, "WOOLWORTHS 5678 PERTH ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(3480), 2L },
                    { 90L, -99.99m, "games-and-software", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(5197), "AUD", new DateOnly(2024, 11, 25), null, "APPLE.COM SYDNEY ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(5197), 2L },
                    { 91L, -130.0m, "fitness-and-wellbeing", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(5739), "AUD", new DateOnly(2024, 11, 26), null, "REBEL SPORT BRISBANE ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(5740), 2L },
                    { 92L, -50.0m, "mobile-phone", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(6434), "AUD", new DateOnly(2024, 11, 27), null, "OPTUS BILL PAYMENT ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(6434), 2L },
                    { 96L, -75.0m, "home-maintenance-and-improvements", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(8565), "AUD", new DateOnly(2024, 12, 1), null, "BUNNINGS WAREHOUSE PERTH ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(8565), 2L },
                    { 97L, -20.0m, "restaurants-and-cafes", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(9141), "AUD", new DateOnly(2024, 12, 2), null, "SUBWAY FRANCHISE ADELAIDE ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(9142), 2L },
                    { 98L, -250.0m, "clothing-and-accessories", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(9665), "AUD", new DateOnly(2024, 12, 3), null, "WESTFIELD SHOPPING CENTRE PERTH ;", new DateTime(2024, 12, 3, 11, 55, 41, 437, DateTimeKind.Utc).AddTicks(9666), 2L },
                    { 100L, -190.0m, "groceries", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(657), "AUD", new DateOnly(2024, 11, 21), "Groceries", "COLES SUPERMARKET ADELAIDE ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(658), 3L },
                    { 104L, -80.0m, "booze", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(2723), "AUD", new DateOnly(2024, 11, 26), null, "DAN MURPHY'S BRISBANE ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(2723), 3L },
                    { 105L, -15.99m, "tv-and-music", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(3169), "AUD", new DateOnly(2024, 11, 27), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(3169), 3L },
                    { 106L, -350.0m, "homeware-and-appliances", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(3761), "AUD", new DateOnly(2024, 11, 28), null, "HARVEY NORMAN PERTH ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(3762), 3L },
                    { 109L, -10.0m, "restaurants-and-cafes", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(5319), "AUD", new DateOnly(2024, 12, 1), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 12, 3, 11, 55, 41, 438, DateTimeKind.Utc).AddTicks(5319), 3L }
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
