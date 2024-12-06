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
                    { "credit", false, new DateTime(2024, 12, 6, 4, 2, 59, 238, DateTimeKind.Utc).AddTicks(6126), "Credit", new DateTime(2024, 12, 6, 4, 2, 59, 238, DateTimeKind.Utc).AddTicks(6406) },
                    { "expense", false, new DateTime(2024, 12, 6, 4, 2, 59, 238, DateTimeKind.Utc).AddTicks(7803), "Expense", new DateTime(2024, 12, 6, 4, 2, 59, 238, DateTimeKind.Utc).AddTicks(7803) },
                    { "none", false, new DateTime(2024, 12, 6, 4, 2, 59, 238, DateTimeKind.Utc).AddTicks(8538), "None", new DateTime(2024, 12, 6, 4, 2, 59, 238, DateTimeKind.Utc).AddTicks(8539) },
                    { "transfer", false, new DateTime(2024, 12, 6, 4, 2, 59, 238, DateTimeKind.Utc).AddTicks(8219), "Transfer", new DateTime(2024, 12, 6, 4, 2, 59, 238, DateTimeKind.Utc).AddTicks(8220) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 12, 6, 4, 2, 59, 244, DateTimeKind.Utc).AddTicks(1363), "user@example.com", "John", "Doe", "$2a$11$ww0zIi7YBilU2zPhGwLVuuaiPyAOXxTL.JzyxYcotPwJEukgjcCVC", new DateTime(2024, 12, 6, 4, 2, 59, 244, DateTimeKind.Utc).AddTicks(1363) },
                    { 2L, new DateTime(2024, 12, 6, 4, 2, 59, 447, DateTimeKind.Utc).AddTicks(9311), "user2@example.com", "Jane", "Smith", "$2a$11$1vzTui/Y9gwmMhHDflQHFug/xb2ClrBAXkVzDCMrGPOPr6AJydxya", new DateTime(2024, 12, 6, 4, 2, 59, 447, DateTimeKind.Utc).AddTicks(9311) },
                    { 3L, new DateTime(2024, 12, 6, 4, 2, 59, 567, DateTimeKind.Utc).AddTicks(3893), "user3@example.com", "Bob", "Johnson", "$2a$11$gzK54s7Cf9nbryALahg8jeIfzRFvFwkye4rp8kDeZst0rKHTjbUOO", new DateTime(2024, 12, 6, 4, 2, 59, 567, DateTimeKind.Utc).AddTicks(3893) }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "TransactionTypeId", "UpdatedAt" },
                values: new object[,]
                {
                    { "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(2778), "Direct Credit", null, "credit", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(2778) },
                    { "good-life", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(7847), "Good Life", null, "expense", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(7847) },
                    { "home", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(1319), "Home", null, "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(1319) },
                    { "none", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(3097), "None", null, "none", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(3097) },
                    { "personal", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(9931), "Personal", null, "expense", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(9931) },
                    { "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(2385), "Transfer", null, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(2385) },
                    { "transport", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(3558), "Transport", null, "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(3558) }
                });

            migrationBuilder.InsertData(
                table: "BudgetLimits",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "PeriodType", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1L, 10000m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(5883), "monthly", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(6138), 1L },
                    { 11L, 5000m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(452), "monthly", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(453), 1L },
                    { 12L, 50m, "transport", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(781), "weekly", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(781), 1L }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "TransactionTypeId", "UpdatedAt" },
                values: new object[,]
                {
                    { "adult", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(1711), "Adult", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(1712) },
                    { "booze", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(8535), "Booze", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(8535) },
                    { "car-insurance-and-maintenance", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(7043), "Car Insurance, Rego & Maintenance", "transport", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(7043) },
                    { "car-repayments", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(6418), "Car Repayments", "transport", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(6418) },
                    { "clothing-and-accessories", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(8871), "Clothing & Accessories", "personal", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(8871) },
                    { "cycling", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(9233), "Cycling", "transport", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(9234) },
                    { "education-and-student-loans", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(286), "Education & Student Loans", "personal", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(286) },
                    { "events-and-gigs", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(637), "Events & Gigs", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(637) },
                    { "family", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(7494), "Children & Family", "personal", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(7494) },
                    { "fitness-and-wellbeing", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(2022), "Fitness & Wellbeing", "personal", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(2023) },
                    { "fuel", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(1011), "Fuel", "transport", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(1011) },
                    { "games-and-software", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(5132), "Apps, Games & Software", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(5803) },
                    { "gifts-and-charity", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(3914), "Gifts & Charity", "personal", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(3914) },
                    { "groceries", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(8145), "Groceries", "home", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(8145) },
                    { "hair-and-beauty", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(5344), "Hair & Beauty", "personal", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(5345) },
                    { "health-and-medical", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(6741), "Health & Medical", "personal", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(6742) },
                    { "hobbies", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(2386), "Hobbies", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(2386) },
                    { "holidays-and-travel", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(4302), "Holidays & Travel", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(4302) },
                    { "home-insurance-and-rates", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(6030), "Rates & Insurance", "home", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(6030) },
                    { "home-maintenance-and-improvements", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(2733), "Maintenance & Improvements", "home", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(2734) },
                    { "homeware-and-appliances", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(9579), "Homeware & Appliances", "home", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 241, DateTimeKind.Utc).AddTicks(9579) },
                    { "internet", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(1659), "Internet", "home", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(1660) },
                    { "investments", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(8201), "Investments", "personal", "credit", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(8201) },
                    { "life-admin", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(9593), "Life Admin", "personal", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(9593) },
                    { "lottery-and-gambling", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(5698), "Lottery & Gambling", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(5698) },
                    { "mobile-phone", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(318), "Mobile Phone", "personal", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(319) },
                    { "news-magazines-and-books", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(1028), "News, Magazines & Books", "personal", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(1028) },
                    { "parking", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(3207), "Parking", "transport", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(3208) },
                    { "pets", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(4621), "Pets", "home", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(4621) },
                    { "public-transport", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(4988), "Public Transport", "transport", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(4988) },
                    { "pubs-and-bars", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(7123), "Pubs & Bars", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(7124) },
                    { "rent-and-mortgage", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(7471), "Rent & Mortgage", "home", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(7471) },
                    { "restaurants-and-cafes", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(8545), "Restaurants & Cafes", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(8545) },
                    { "takeaway", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(9899), "Takeaway", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(9899) },
                    { "taxis-and-share-cars", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(7847), "Taxis & Share Cars", "transport", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(7847) },
                    { "technology", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(2051), "Technology", "personal", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(2051) },
                    { "tobacco-and-vaping", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(638), "Tobacco & Vaping", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(638) },
                    { "toll-roads", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(8903), "Tolls", "transport", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(8903) },
                    { "tv-and-music", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(1371), "TV, Music & Streaming", "good-life", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 243, DateTimeKind.Utc).AddTicks(1372) },
                    { "utilities", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(9199), "Utilities", "home", "expense", new DateTime(2024, 12, 6, 4, 2, 59, 242, DateTimeKind.Utc).AddTicks(9199) }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "Currency", "Date", "ModifiedDescription", "OriginalDescription", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 2L, -150.0m, "none", new DateTime(2024, 12, 6, 4, 2, 59, 691, DateTimeKind.Utc).AddTicks(7881), "AUD", new DateOnly(2024, 10, 22), "Woolworths Groceries", "WOOLWORTHS 1234 SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 691, DateTimeKind.Utc).AddTicks(7881), 1L },
                    { 5L, -15.99m, "none", new DateTime(2024, 12, 6, 4, 2, 59, 691, DateTimeKind.Utc).AddTicks(9452), "AUD", new DateOnly(2024, 10, 27), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 12, 6, 4, 2, 59, 691, DateTimeKind.Utc).AddTicks(9452), 1L },
                    { 6L, 3000.0m, "none", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(79), "AUD", new DateOnly(2024, 10, 30), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(79), 1L },
                    { 7L, -500.0m, "none", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(512), "AUD", new DateOnly(2024, 10, 31), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(512), 1L },
                    { 16L, 3200.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(4740), "AUD", new DateOnly(2024, 10, 29), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(4740), 2L },
                    { 17L, -800.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(5218), "AUD", new DateOnly(2024, 10, 31), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(5218), 2L },
                    { 21L, -23.45m, "transport", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(6997), "AUD", new DateOnly(2024, 10, 18), "Uber Rides", "UBER *RIDES SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(6997), 3L },
                    { 26L, 2900.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(9381), "AUD", new DateOnly(2024, 10, 29), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(9381), 3L },
                    { 27L, -1000.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(9880), "AUD", new DateOnly(2024, 10, 30), null, "TRANSFER TO MORTGAGE ACCOUNT ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(9880), 3L },
                    { 33L, -150.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(3109), "AUD", new DateOnly(2024, 11, 6), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(3109), 1L },
                    { 36L, 600.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(4401), "AUD", new DateOnly(2024, 11, 5), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(4401), 2L },
                    { 37L, -250.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(4790), "AUD", new DateOnly(2024, 11, 6), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(4790), 2L },
                    { 39L, -1200.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(5686), "AUD", new DateOnly(2024, 11, 6), null, "BOQ HOME LOAN PAYMENT ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(5687), 3L },
                    { 40L, 500.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(6143), "AUD", new DateOnly(2024, 11, 8), null, "TRANSFER FROM INVESTMENT ACCOUNT ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(6143), 3L },
                    { 41L, -200.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(6555), "AUD", new DateOnly(2024, 11, 9), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(6555), 3L },
                    { 42L, 1500.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(6989), "AUD", new DateOnly(2024, 11, 10), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(6990), 3L },
                    { 45L, 1000.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(8270), "AUD", new DateOnly(2024, 11, 10), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(8270), 2L },
                    { 49L, -150.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(9989), "AUD", new DateOnly(2024, 11, 14), null, "TRANSFER TO FRIEND ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(9989), 1L },
                    { 51L, -200.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(846), "AUD", new DateOnly(2024, 11, 13), null, "TRANSFER TO FAMILY ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(846), 2L },
                    { 52L, -500.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(1249), "AUD", new DateOnly(2024, 11, 14), null, "TRANSFER TO SUPERANNUATION ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(1250), 2L },
                    { 53L, 300.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(1686), "AUD", new DateOnly(2024, 11, 15), null, "TRANSFER FROM SAVINGS ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(1686), 3L },
                    { 55L, 3200.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(2587), "AUD", new DateOnly(2024, 11, 17), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(2588), 1L },
                    { 57L, 500.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(3429), "AUD", new DateOnly(2024, 11, 19), "Bonus", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(3429), 1L },
                    { 59L, 3300.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(4276), "AUD", new DateOnly(2024, 11, 17), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(4276), 2L },
                    { 61L, 700.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(5164), "AUD", new DateOnly(2024, 11, 19), "Performance Bonus", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(5164), 2L },
                    { 62L, 30.0m, "none", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(5589), "AUD", new DateOnly(2024, 11, 20), "Fuel Refund", "REFUND FROM BP ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(5589), 2L },
                    { 63L, 3100.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(6046), "AUD", new DateOnly(2024, 11, 17), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(6046), 3L },
                    { 65L, 400.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(6908), "AUD", new DateOnly(2024, 11, 19), "Overtime Payment", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(6908), 3L },
                    { 67L, 3250.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(7783), "AUD", new DateOnly(2024, 11, 21), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(7783), 1L },
                    { 69L, 3400.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(8652), "AUD", new DateOnly(2024, 11, 21), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(8652), 2L },
                    { 71L, 3200.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(9521), "AUD", new DateOnly(2024, 11, 21), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(9521), 3L },
                    { 73L, -25.5m, "transport", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(366), "AUD", new DateOnly(2024, 11, 23), "Uber Rides", "UBER *RIDES BRISBANE ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(366), 1L },
                    { 75L, 3000.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(1244), "AUD", new DateOnly(2024, 11, 26), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(1245), 1L },
                    { 76L, -600.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(1664), "AUD", new DateOnly(2024, 11, 27), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(1664), 1L },
                    { 81L, -200.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(3858), "AUD", new DateOnly(2024, 12, 2), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(3858), 1L },
                    { 82L, 3200.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(4303), "AUD", new DateOnly(2024, 12, 3), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(4304), 1L },
                    { 88L, 3400.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(6867), "AUD", new DateOnly(2024, 11, 26), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(6867), 2L },
                    { 89L, -900.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(7273), "AUD", new DateOnly(2024, 11, 27), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(7273), 2L },
                    { 93L, 800.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(8970), "AUD", new DateOnly(2024, 12, 1), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(8971), 2L },
                    { 94L, -300.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(9429), "AUD", new DateOnly(2024, 12, 2), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(9430), 2L },
                    { 95L, 3300.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(9840), "AUD", new DateOnly(2024, 12, 3), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(9841), 2L },
                    { 99L, -35.0m, "transport", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(1567), "AUD", new DateOnly(2024, 11, 23), "Uber Rides", "UBER *RIDES SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(1567), 3L },
                    { 101L, 3100.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(2426), "AUD", new DateOnly(2024, 11, 26), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(2427), 3L },
                    { 102L, -1200.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(2843), "AUD", new DateOnly(2024, 11, 27), null, "TRANSFER TO MORTGAGE ACCOUNT ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(2843), 3L },
                    { 103L, -550.0m, "none", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(3266), "AUD", new DateOnly(2024, 11, 28), null, "JB HI-FI MELBOURNE ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(3267), 3L },
                    { 107L, -250.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(5002), "AUD", new DateOnly(2024, 12, 2), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(5002), 3L },
                    { 108L, 1700.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(5432), "AUD", new DateOnly(2024, 12, 3), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(5433), 3L },
                    { 110L, 3200.0m, "direct-credit", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(6305), "AUD", new DateOnly(2024, 12, 5), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(6305), 3L },
                    { 111L, -1300.0m, "transfer", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(6765), "AUD", new DateOnly(2024, 12, 6), null, "BOQ HOME LOAN PAYMENT ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(6765), 3L }
                });

            migrationBuilder.InsertData(
                table: "BudgetLimits",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "PeriodType", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 2L, 250m, "groceries", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(7054), "weekly", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(7054), 1L },
                    { 3L, 150m, "games-and-software", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(7430), "quarterly", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(7430), 1L },
                    { 4L, 400m, "clothing-and-accessories", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(7808), "quarterly", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(7808), 1L },
                    { 5L, 100m, "fuel", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(8151), "weekly", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(8151), 1L },
                    { 6L, 50m, "tv-and-music", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(8541), "monthly", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(8541), 1L },
                    { 7L, 1800m, "technology", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(8916), "annual", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(8917), 1L },
                    { 8L, 3000m, "home-maintenance-and-improvements", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(9264), "annual", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(9265), 1L },
                    { 9L, 120m, "restaurants-and-cafes", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(9662), "weekly", new DateTime(2024, 12, 6, 4, 2, 59, 697, DateTimeKind.Utc).AddTicks(9662), 1L },
                    { 10L, 300m, "utilities", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(36), "monthly", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(36), 1L },
                    { 13L, 2000m, "holidays-and-travel", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(1179), "annual", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(1179), 1L },
                    { 14L, 300m, "fitness-and-wellbeing", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(1531), "monthly", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(1531), 2L },
                    { 15L, 50m, "mobile-phone", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(1863), "monthly", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(1863), 2L },
                    { 16L, 200m, "booze", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(2265), "monthly", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(2266), 3L },
                    { 17L, 800m, "homeware-and-appliances", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(2592), "annual", new DateTime(2024, 12, 6, 4, 2, 59, 698, DateTimeKind.Utc).AddTicks(2592), 3L }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "Currency", "Date", "ModifiedDescription", "OriginalDescription", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1L, -45.6m, "restaurants-and-cafes", new DateTime(2024, 12, 6, 4, 2, 59, 691, DateTimeKind.Utc).AddTicks(6378), "AUD", new DateOnly(2024, 10, 20), "Uber Eats Delivery", "UBER *EATS HELP.UBER.COM ;", new DateTime(2024, 12, 6, 4, 2, 59, 691, DateTimeKind.Utc).AddTicks(6645), 1L },
                    { 3L, -200.0m, "clothing-and-accessories", new DateTime(2024, 12, 6, 4, 2, 59, 691, DateTimeKind.Utc).AddTicks(8456), "AUD", new DateOnly(2024, 10, 24), null, "MYER DEPARTMENT STORE SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 691, DateTimeKind.Utc).AddTicks(8456), 1L },
                    { 4L, -450.0m, "holidays-and-travel", new DateTime(2024, 12, 6, 4, 2, 59, 691, DateTimeKind.Utc).AddTicks(8987), "AUD", new DateOnly(2024, 10, 26), "Flights to Melbourne", "FLIGHT CENTRE MELBOURNE ;", new DateTime(2024, 12, 6, 4, 2, 59, 691, DateTimeKind.Utc).AddTicks(8987), 1L },
                    { 8L, 200.0m, "gifts-and-charity", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(1073), "AUD", new DateOnly(2024, 11, 1), "Gift from Parents", "TRANSFER FROM PARENTS ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(1073), 1L },
                    { 9L, -75.5m, "home-maintenance-and-improvements", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(1535), "AUD", new DateOnly(2024, 11, 3), null, "BUNNINGS WAREHOUSE BRISBANE ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(1536), 1L },
                    { 10L, -120.0m, "technology", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(1999), "AUD", new DateOnly(2024, 11, 4), null, "AMAZON AU MELBOURNE ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(1999), 1L },
                    { 11L, -60.0m, "fuel", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(2455), "AUD", new DateOnly(2024, 10, 19), null, "SHELL SERVICE STATION PERTH ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(2455), 2L },
                    { 12L, -70.0m, "fuel", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(2837), "AUD", new DateOnly(2024, 10, 21), null, "BP AUSTRALIA PERTH ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(2837), 2L },
                    { 13L, -99.99m, "games-and-software", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(3413), "AUD", new DateOnly(2024, 10, 23), null, "APPLE.COM SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(3414), 2L },
                    { 14L, -250.0m, "groceries", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(3832), "AUD", new DateOnly(2024, 10, 24), null, "WOOLWORTHS 5678 PERTH ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(3832), 2L },
                    { 15L, -15.0m, "parking", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(4331), "AUD", new DateOnly(2024, 10, 25), null, "CITY OF PERTH PARKING ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(4331), 2L },
                    { 18L, 500.0m, "gifts-and-charity", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(5636), "AUD", new DateOnly(2024, 11, 1), "Loan from Sister", "TRANSFER FROM SISTER ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(5637), 2L },
                    { 19L, -120.0m, "fitness-and-wellbeing", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(6120), "AUD", new DateOnly(2024, 11, 2), null, "REBEL SPORT BRISBANE ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(6120), 2L },
                    { 20L, -45.0m, "mobile-phone", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(6513), "AUD", new DateOnly(2024, 11, 4), null, "OPTUS BILL PAYMENT ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(6513), 2L },
                    { 22L, -180.0m, "groceries", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(7415), "AUD", new DateOnly(2024, 10, 20), "Groceries", "COLES SUPERMARKET ADELAIDE ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(7415), 3L },
                    { 23L, -500.0m, "technology", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(8059), "AUD", new DateOnly(2024, 10, 22), null, "JB HI-FI MELBOURNE ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(8060), 3L },
                    { 24L, -320.0m, "homeware-and-appliances", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(8493), "AUD", new DateOnly(2024, 10, 25), null, "HARVEY NORMAN PERTH ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(8493), 3L },
                    { 25L, -15.99m, "tv-and-music", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(8983), "AUD", new DateOnly(2024, 10, 27), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 12, 6, 4, 2, 59, 692, DateTimeKind.Utc).AddTicks(8983), 3L },
                    { 28L, 100.0m, "gifts-and-charity", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(780), "AUD", new DateOnly(2024, 11, 1), "Gift from Friend", "TRANSFER FROM FRIEND ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(780), 3L },
                    { 29L, -40.0m, "clothing-and-accessories", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(1380), "AUD", new DateOnly(2024, 11, 2), null, "BIG W SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(1380), 3L },
                    { 30L, -60.0m, "booze", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(1825), "AUD", new DateOnly(2024, 11, 3), null, "DAN MURPHY'S BRISBANE ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(1825), 3L },
                    { 31L, -60.0m, "groceries", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(2285), "AUD", new DateOnly(2024, 11, 4), null, "WOOLWORTHS 9999 BRISBANE ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(2285), 1L },
                    { 32L, -25.0m, "parking", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(2660), "AUD", new DateOnly(2024, 11, 5), null, "CITY OF SYDNEY PARKING ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(2660), 1L },
                    { 34L, -70.0m, "utilities", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(3524), "AUD", new DateOnly(2024, 11, 7), null, "TELSTRA BILL PAYMENT ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(3524), 1L },
                    { 35L, -200.0m, "clothing-and-accessories", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(3966), "AUD", new DateOnly(2024, 11, 4), null, "WESTFIELD SHOPPING CENTRE PERTH ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(3966), 2L },
                    { 38L, -15.0m, "restaurants-and-cafes", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(5272), "AUD", new DateOnly(2024, 11, 7), null, "SUBWAY FRANCHISE ADELAIDE ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(5273), 2L },
                    { 43L, -900.0m, "rent-and-mortgage", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(7409), "AUD", new DateOnly(2024, 11, 11), null, "AUSSIE HOME LOANS SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(7410), 1L },
                    { 44L, -8.0m, "restaurants-and-cafes", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(7846), "AUD", new DateOnly(2024, 11, 12), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(7847), 1L },
                    { 46L, -60.0m, "home-maintenance-and-improvements", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(8734), "AUD", new DateOnly(2024, 11, 11), null, "BUNNINGS WAREHOUSE PERTH ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(8734), 2L },
                    { 47L, -120.0m, "groceries", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(9147), "AUD", new DateOnly(2024, 11, 12), "Groceries", "COLES SUPERMARKET MELBOURNE ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(9147), 3L },
                    { 48L, -250.0m, "technology", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(9568), "AUD", new DateOnly(2024, 11, 13), null, "KOGAN.COM SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 693, DateTimeKind.Utc).AddTicks(9569), 3L },
                    { 50L, -60.0m, "restaurants-and-cafes", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(422), "AUD", new DateOnly(2024, 11, 15), "Tickets", "MELBOURNE ZOO MELBOURNE ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(422), 1L },
                    { 54L, -100.0m, "health-and-medical", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(2114), "AUD", new DateOnly(2024, 11, 16), null, "MEDIBANK PRIVATE SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(2115), 3L },
                    { 56L, 150.0m, "gifts-and-charity", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(3011), "AUD", new DateOnly(2024, 11, 18), "Returned Item Refund", "REFUND FROM AMAZON ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(3011), 1L },
                    { 58L, 20.0m, "groceries", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(3813), "AUD", new DateOnly(2024, 11, 20), "Groceries Refund", "REFUND FROM WOOLWORTHS ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(3813), 1L },
                    { 60L, 10.0m, "games-and-software", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(4706), "AUD", new DateOnly(2024, 11, 18), "App Refund", "REFUND FROM APPLE ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(4706), 2L },
                    { 64L, 50.0m, "clothing-and-accessories", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(6458), "AUD", new DateOnly(2024, 11, 18), "Clothing Refund", "REFUND FROM BIG W ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(6458), 3L },
                    { 66L, 100.0m, "technology", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(7338), "AUD", new DateOnly(2024, 11, 20), "Technology Refund", "REFUND FROM JB HI-FI ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(7338), 3L },
                    { 68L, 15.99m, "tv-and-music", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(8207), "AUD", new DateOnly(2024, 11, 22), "Subscription Refund", "REFUND FROM NETFLIX ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(8207), 1L },
                    { 70L, 25.0m, "fuel", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(9044), "AUD", new DateOnly(2024, 11, 22), "Fuel Refund", "REFUND FROM SHELL ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(9045), 2L },
                    { 72L, 15.0m, "groceries", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(9951), "AUD", new DateOnly(2024, 11, 22), "Groceries Refund", "REFUND FROM COLES ;", new DateTime(2024, 12, 6, 4, 2, 59, 694, DateTimeKind.Utc).AddTicks(9951), 3L },
                    { 74L, -180.0m, "groceries", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(831), "AUD", new DateOnly(2024, 11, 24), "Woolworths Groceries", "WOOLWORTHS 1234 SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(831), 1L },
                    { 77L, -220.0m, "technology", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(2078), "AUD", new DateOnly(2024, 11, 28), null, "AMAZON AU SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(2078), 1L },
                    { 78L, -90.0m, "home-maintenance-and-improvements", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(2519), "AUD", new DateOnly(2024, 11, 29), null, "BUNNINGS WAREHOUSE BRISBANE ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(2520), 1L },
                    { 79L, -9.5m, "restaurants-and-cafes", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(2967), "AUD", new DateOnly(2024, 11, 30), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(2968), 1L },
                    { 80L, -85.0m, "utilities", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(3407), "AUD", new DateOnly(2024, 12, 1), null, "TELSTRA BILL PAYMENT ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(3407), 1L },
                    { 83L, -300.0m, "clothing-and-accessories", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(4666), "AUD", new DateOnly(2024, 12, 4), null, "MYER DEPARTMENT STORE SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(4667), 1L },
                    { 84L, -55.0m, "restaurants-and-cafes", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(5136), "AUD", new DateOnly(2024, 12, 5), null, "UBER *EATS SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(5136), 1L },
                    { 85L, -480.0m, "holidays-and-travel", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(5531), "AUD", new DateOnly(2024, 12, 6), "Flights to Sydney", "FLIGHT CENTRE SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(5531), 1L },
                    { 86L, -80.0m, "fuel", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(5991), "AUD", new DateOnly(2024, 11, 23), null, "BP SERVICE STATION PERTH ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(5992), 2L },
                    { 87L, -220.0m, "groceries", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(6379), "AUD", new DateOnly(2024, 11, 24), null, "WOOLWORTHS 5678 PERTH ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(6379), 2L },
                    { 90L, -99.99m, "games-and-software", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(7728), "AUD", new DateOnly(2024, 11, 28), null, "APPLE.COM SYDNEY ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(7728), 2L },
                    { 91L, -130.0m, "fitness-and-wellbeing", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(8133), "AUD", new DateOnly(2024, 11, 29), null, "REBEL SPORT BRISBANE ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(8134), 2L },
                    { 92L, -50.0m, "mobile-phone", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(8562), "AUD", new DateOnly(2024, 11, 30), null, "OPTUS BILL PAYMENT ;", new DateTime(2024, 12, 6, 4, 2, 59, 695, DateTimeKind.Utc).AddTicks(8562), 2L },
                    { 96L, -75.0m, "home-maintenance-and-improvements", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(321), "AUD", new DateOnly(2024, 12, 4), null, "BUNNINGS WAREHOUSE PERTH ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(322), 2L },
                    { 97L, -20.0m, "restaurants-and-cafes", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(726), "AUD", new DateOnly(2024, 12, 5), null, "SUBWAY FRANCHISE ADELAIDE ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(726), 2L },
                    { 98L, -250.0m, "clothing-and-accessories", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(1173), "AUD", new DateOnly(2024, 12, 6), null, "WESTFIELD SHOPPING CENTRE PERTH ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(1173), 2L },
                    { 100L, -190.0m, "groceries", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(1971), "AUD", new DateOnly(2024, 11, 24), "Groceries", "COLES SUPERMARKET ADELAIDE ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(1972), 3L },
                    { 104L, -80.0m, "booze", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(3713), "AUD", new DateOnly(2024, 11, 29), null, "DAN MURPHY'S BRISBANE ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(3713), 3L },
                    { 105L, -15.99m, "tv-and-music", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(4158), "AUD", new DateOnly(2024, 11, 30), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(4158), 3L },
                    { 106L, -350.0m, "homeware-and-appliances", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(4580), "AUD", new DateOnly(2024, 12, 1), null, "HARVEY NORMAN PERTH ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(4580), 3L },
                    { 109L, -10.0m, "restaurants-and-cafes", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(5874), "AUD", new DateOnly(2024, 12, 4), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 12, 6, 4, 2, 59, 696, DateTimeKind.Utc).AddTicks(5875), 3L }
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
