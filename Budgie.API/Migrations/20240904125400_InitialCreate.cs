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
                    { "direct_credit", false, new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(3419), "Direct Credit", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(3420) },
                    { "international_purchase", false, new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(2924), "International Purchase", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(2924) },
                    { "payment", false, new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(4288), "Payment", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(4288) },
                    { "purchase", false, new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(2426), "Purchase", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(2426) },
                    { "refund", false, new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(3858), "Refund", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(3859) },
                    { "transfer", false, new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(4668), "Transfer", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(4669) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(7718), "user@example.com", "John", "Doe", "$2a$11$T1z0uc3wTz4heE7E2sMuFesoXyWnp4E5xJk0Sl.DdMYrvMlUk9jbO", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(7719) },
                    { 2L, new DateTime(2024, 9, 4, 12, 53, 59, 308, DateTimeKind.Utc).AddTicks(1768), "user2@example.com", "Jane", "Smith", "$2a$11$uWQuejCzKXuMzFkumEXRHuxGNXkbr4AlzbeRYmG.ZPpKlsikin90u", new DateTime(2024, 9, 4, 12, 53, 59, 308, DateTimeKind.Utc).AddTicks(1768) },
                    { 3L, new DateTime(2024, 9, 4, 12, 53, 59, 446, DateTimeKind.Utc).AddTicks(4205), "user3@example.com", "Bob", "Johnson", "$2a$11$BObAZD/j21p8iGN2edkYGu274D2BrjRsM4rDrMghCeHyqgaKnIjWG", new DateTime(2024, 9, 4, 12, 53, 59, 446, DateTimeKind.Utc).AddTicks(4206) }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "TransactionTypeId", "UpdatedAt" },
                values: new object[,]
                {
                    { "good-life", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(7833), "Good Life", null, "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(7833) },
                    { "home", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(2438), "Home", null, "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(2438) },
                    { "personal", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(579), "Personal", null, "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(579) },
                    { "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(6597), "Transfer", null, "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(6597) },
                    { "transport", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(5129), "Transport", null, "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(5130) },
                    { "adult", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(5694), "Adult", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(5695) },
                    { "booze", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(8760), "Booze", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(8760) },
                    { "car-insurance-and-maintenance", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(6715), "Car Insurance, Rego & Maintenance", "transport", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(6715) },
                    { "car-repayments", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(8776), "Repayments", "transport", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(8777) },
                    { "clothing-and-accessories", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(9184), "Clothing & Accessories", "personal", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(9184) },
                    { "cycling", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(9671), "Cycling", "transport", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(9671) },
                    { "education-and-student-loans", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(1062), "Education & Student Loans", "personal", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(1062) },
                    { "events-and-gigs", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(1461), "Events & Gigs", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(1461) },
                    { "family", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(7284), "Children & Family", "personal", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(7285) },
                    { "fitness-and-wellbeing", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(3325), "Fitness & Wellbeing", "personal", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(3325) },
                    { "fuel", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(1983), "Fuel", "transport", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(1983) },
                    { "games-and-software", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(5904), "Apps, Games & Software", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(5905) },
                    { "gifts-and-charity", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(5608), "Gifts & Charity", "personal", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(5609) },
                    { "groceries", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(8256), "Groceries", "home", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 166, DateTimeKind.Utc).AddTicks(8256) },
                    { "hair-and-beauty", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(7420), "Hair & Beauty", "personal", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(7421) },
                    { "health-and-medical", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(9219), "Health & Medical", "personal", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(9219) },
                    { "hobbies", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(3789), "Hobbies", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(3789) },
                    { "holidays-and-travel", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(6061), "Holidays & Travel", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(6062) },
                    { "home-insurance-and-rates", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(8306), "Rates & Insurance", "home", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(8307) },
                    { "home-maintenance-and-improvements", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(4196), "Maintenance & Improvements", "home", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(4196) },
                    { "homeware-and-appliances", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(146), "Homeware & Appliances", "home", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(147) },
                    { "internet", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(2849), "Internet", "home", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(2849) },
                    { "investments", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(1064), "Investments", "personal", "direct_credit", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(1064) },
                    { "life-admin", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(2858), "Life Admin", "personal", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(2858) },
                    { "lottery-and-gambling", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(7863), "Lottery & Gambling", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(7863) },
                    { "mobile-phone", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(3784), "Mobile Phone", "personal", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(3784) },
                    { "news-magazines-and-books", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(4791), "News, Magazines & Books", "personal", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(4791) },
                    { "parking", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(4700), "Parking", "transport", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(4701) },
                    { "pets", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(6484), "Pets", "home", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(6484) },
                    { "public-transport", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(6960), "Public Transport", "transport", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(6960) },
                    { "pubs-and-bars", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(9733), "Pubs & Bars", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 167, DateTimeKind.Utc).AddTicks(9733) },
                    { "rent-and-mortgage", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(143), "Rent & Mortgage", "home", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(144) },
                    { "restaurants-and-cafes", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(1517), "Restaurants & Cafes", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(1517) },
                    { "takeaway", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(3322), "Takeaway", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(3322) },
                    { "taxis-and-share-cars", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(638), "Taxis & Share Cars", "transport", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(639) },
                    { "technology", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(6143), "Technology", "personal", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(6144) },
                    { "tobacco-and-vaping", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(4247), "Tobacco & Vaping", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(4247) },
                    { "toll-roads", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(2001), "Tolls", "transport", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(2002) },
                    { "tv-and-music", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(5241), "TV, Music & Streaming", "good-life", "purchase", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(5241) },
                    { "utilities", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(2423), "Utilities", "home", "payment", new DateTime(2024, 9, 4, 12, 53, 59, 168, DateTimeKind.Utc).AddTicks(2424) }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "Currency", "Date", "ModifiedDescription", "OriginalDescription", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 7L, 500.00m, "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(7610), "AUD", new DateOnly(2023, 8, 7), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(7610), 1L },
                    { 17L, 800.00m, "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(7177), "AUD", new DateOnly(2023, 8, 7), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(7177), 2L },
                    { 21L, -23.45m, "transport", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(9695), "AUD", new DateOnly(2023, 8, 1), "Uber Rides", "UBER *RIDES SYDNEY ;", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(9695), 3L },
                    { 27L, 1000.00m, "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(5456), "AUD", new DateOnly(2023, 8, 7), null, "TRANSFER TO MORTGAGE ACCOUNT ;", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(5456), 3L },
                    { 33L, 150.00m, "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(9728), "AUD", new DateOnly(2023, 8, 13), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(9728), 1L },
                    { 37L, 250.00m, "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(3284), "AUD", new DateOnly(2023, 8, 13), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(3285), 2L },
                    { 39L, 1200.00m, "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(4284), "AUD", new DateOnly(2023, 8, 11), null, "BOQ HOME LOAN PAYMENT ;", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(4284), 3L },
                    { 41L, 200.00m, "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(5191), "AUD", new DateOnly(2023, 8, 13), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(5191), 3L },
                    { 49L, 150.00m, "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 611, DateTimeKind.Utc).AddTicks(107), "AUD", new DateOnly(2023, 8, 17), null, "TRANSFER TO FRIEND ;", new DateTime(2024, 9, 4, 12, 53, 59, 611, DateTimeKind.Utc).AddTicks(107), 1L },
                    { 51L, 200.00m, "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 611, DateTimeKind.Utc).AddTicks(1621), "AUD", new DateOnly(2023, 8, 17), null, "TRANSFER TO FAMILY ;", new DateTime(2024, 9, 4, 12, 53, 59, 611, DateTimeKind.Utc).AddTicks(1622), 2L },
                    { 52L, 500.00m, "transfer", new DateTime(2024, 9, 4, 12, 53, 59, 611, DateTimeKind.Utc).AddTicks(2145), "AUD", new DateOnly(2023, 8, 18), null, "TRANSFER TO SUPERANNUATION ;", new DateTime(2024, 9, 4, 12, 53, 59, 611, DateTimeKind.Utc).AddTicks(2145), 2L },
                    { 1L, -45.60m, "restaurants-and-cafes", new DateTime(2024, 9, 4, 12, 53, 59, 606, DateTimeKind.Utc).AddTicks(9667), "AUD", new DateOnly(2023, 8, 1), "Uber Eats Delivery", "UBER *EATS HELP.UBER.COM ;", new DateTime(2024, 9, 4, 12, 53, 59, 606, DateTimeKind.Utc).AddTicks(9668), 1L },
                    { 2L, -150.00m, "groceries", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(735), "AUD", new DateOnly(2023, 8, 2), "Woolworths Groceries", "WOOLWORTHS 1234 SYDNEY ;", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(735), 1L },
                    { 3L, 200.00m, "clothing-and-accessories", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(1582), "AUD", new DateOnly(2023, 8, 3), null, "MYER DEPARTMENT STORE SYDNEY ;", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(1582), 1L },
                    { 4L, -450.00m, "holidays-and-travel", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(4181), "AUD", new DateOnly(2023, 8, 4), "Flights to Melbourne", "FLIGHT CENTRE MELBOURNE ;", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(4182), 1L },
                    { 5L, 15.99m, "tv-and-music", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(5059), "AUD", new DateOnly(2023, 8, 5), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(5059), 1L },
                    { 8L, 200.00m, "gifts-and-charity", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(8778), "AUD", new DateOnly(2023, 8, 8), "Gift from Parents", "TRANSFER FROM PARENTS ;", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(8778), 1L },
                    { 9L, 75.50m, "home-maintenance-and-improvements", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(9405), "AUD", new DateOnly(2023, 8, 9), null, "BUNNINGS WAREHOUSE BRISBANE ;", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(9405), 1L },
                    { 10L, 120.00m, "technology", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(9904), "AUD", new DateOnly(2023, 8, 10), null, "AMAZON AU MELBOURNE ;", new DateTime(2024, 9, 4, 12, 53, 59, 607, DateTimeKind.Utc).AddTicks(9904), 1L },
                    { 11L, 60.00m, "fuel", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(2582), "AUD", new DateOnly(2023, 8, 1), null, "SHELL SERVICE STATION PERTH ;", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(2583), 2L },
                    { 12L, 70.00m, "fuel", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(3297), "AUD", new DateOnly(2023, 8, 2), null, "BP AUSTRALIA PERTH ;", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(3298), 2L },
                    { 14L, 250.00m, "groceries", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(5135), "AUD", new DateOnly(2023, 8, 4), null, "WOOLWORTHS 5678 PERTH ;", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(5136), 2L },
                    { 15L, 15.00m, "parking", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(5960), "AUD", new DateOnly(2023, 8, 5), null, "CITY OF PERTH PARKING ;", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(5961), 2L },
                    { 18L, 500.00m, "gifts-and-charity", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(7688), "AUD", new DateOnly(2023, 8, 8), "Loan from Sister", "TRANSFER FROM SISTER ;", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(7688), 2L },
                    { 19L, 120.00m, "fitness-and-wellbeing", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(8096), "AUD", new DateOnly(2023, 8, 9), null, "REBEL SPORT BRISBANE ;", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(8096), 2L },
                    { 20L, 45.00m, "mobile-phone", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(8607), "AUD", new DateOnly(2023, 8, 10), null, "OPTUS BILL PAYMENT ;", new DateTime(2024, 9, 4, 12, 53, 59, 608, DateTimeKind.Utc).AddTicks(8608), 2L },
                    { 22L, -180.00m, "groceries", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(2816), "AUD", new DateOnly(2023, 8, 2), "Groceries", "COLES SUPERMARKET ADELAIDE ;", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(2817), 3L },
                    { 23L, 500.00m, "technology", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(3430), "AUD", new DateOnly(2023, 8, 3), null, "JB HI-FI MELBOURNE ;", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(3430), 3L },
                    { 24L, 320.00m, "homeware-and-appliances", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(3968), "AUD", new DateOnly(2023, 8, 4), null, "HARVEY NORMAN PERTH ;", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(3968), 3L },
                    { 25L, 15.99m, "tv-and-music", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(4505), "AUD", new DateOnly(2023, 8, 5), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(4505), 3L },
                    { 28L, 100.00m, "gifts-and-charity", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(5884), "AUD", new DateOnly(2023, 8, 8), "Gift from Friend", "TRANSFER FROM FRIEND ;", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(5884), 3L },
                    { 29L, 40.00m, "clothing-and-accessories", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(6367), "AUD", new DateOnly(2023, 8, 9), null, "BIG W SYDNEY ;", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(6368), 3L },
                    { 30L, 60.00m, "booze", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(8149), "AUD", new DateOnly(2023, 8, 10), null, "DAN MURPHY'S BRISBANE ;", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(8150), 3L },
                    { 31L, 60.00m, "groceries", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(8736), "AUD", new DateOnly(2023, 8, 11), null, "WOOLWORTHS 9999 BRISBANE ;", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(8736), 1L },
                    { 32L, 25.00m, "parking", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(9148), "AUD", new DateOnly(2023, 8, 12), null, "CITY OF SYDNEY PARKING ;", new DateTime(2024, 9, 4, 12, 53, 59, 609, DateTimeKind.Utc).AddTicks(9148), 1L },
                    { 34L, 70.00m, "utilities", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(1558), "AUD", new DateOnly(2023, 8, 14), null, "TELSTRA BILL PAYMENT ;", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(1559), 1L },
                    { 35L, 200.00m, "clothing-and-accessories", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(2365), "AUD", new DateOnly(2023, 8, 11), null, "WESTFIELD SHOPPING CENTRE PERTH ;", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(2365), 2L },
                    { 38L, 15.00m, "restaurants-and-cafes", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(3800), "AUD", new DateOnly(2023, 8, 14), null, "SUBWAY FRANCHISE ADELAIDE ;", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(3801), 2L },
                    { 43L, 900.00m, "rent-and-mortgage", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(6272), "AUD", new DateOnly(2023, 8, 15), null, "AUSSIE HOME LOANS SYDNEY ;", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(6273), 1L },
                    { 44L, 8.00m, "restaurants-and-cafes", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(6712), "AUD", new DateOnly(2023, 8, 16), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(6713), 1L },
                    { 46L, 60.00m, "home-maintenance-and-improvements", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(7732), "AUD", new DateOnly(2023, 8, 16), null, "BUNNINGS WAREHOUSE PERTH ;", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(7733), 2L },
                    { 47L, -120.00m, "groceries", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(8426), "AUD", new DateOnly(2023, 8, 15), "Groceries", "COLES SUPERMARKET MELBOURNE ;", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(8427), 3L },
                    { 48L, 250.00m, "technology", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(9401), "AUD", new DateOnly(2023, 8, 16), null, "KOGAN.COM SYDNEY ;", new DateTime(2024, 9, 4, 12, 53, 59, 610, DateTimeKind.Utc).AddTicks(9401), 3L },
                    { 50L, -60.00m, "restaurants-and-cafes", new DateTime(2024, 9, 4, 12, 53, 59, 611, DateTimeKind.Utc).AddTicks(1071), "AUD", new DateOnly(2023, 8, 18), "Tickets", "MELBOURNE ZOO MELBOURNE ;", new DateTime(2024, 9, 4, 12, 53, 59, 611, DateTimeKind.Utc).AddTicks(1071), 1L },
                    { 54L, 100.00m, "health-and-medical", new DateTime(2024, 9, 4, 12, 53, 59, 611, DateTimeKind.Utc).AddTicks(3025), "AUD", new DateOnly(2023, 8, 18), null, "MEDIBANK PRIVATE SYDNEY ;", new DateTime(2024, 9, 4, 12, 53, 59, 611, DateTimeKind.Utc).AddTicks(3026), 3L }
                });

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
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
