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
                    { "direct-credit", false, new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(3008), "Direct Credit", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(3009) },
                    { "international-purchase", false, new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(2032), "International Purchase", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(2033) },
                    { "payment", false, new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(4798), "Payment", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(4799) },
                    { "purchase", false, new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(1081), "Purchase", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(1082) },
                    { "refund", false, new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(3953), "Refund", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(3954) },
                    { "transfer", false, new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(5724), "Transfer", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(5725) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(9631), "user@example.com", "John", "Doe", "$2a$11$YOFRr7b0rFSawRljmlDPrOxpLJKOCBmzKmJIyfJfRizTTVlMCwEz.", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(9632) },
                    { 2L, new DateTime(2024, 9, 9, 11, 29, 47, 149, DateTimeKind.Utc).AddTicks(1138), "user2@example.com", "Jane", "Smith", "$2a$11$0zoJq7UvjlELCl3EWgokRuH8nsVdxVv4GPkL4KbhWW0yDmWQ8bs7y", new DateTime(2024, 9, 9, 11, 29, 47, 149, DateTimeKind.Utc).AddTicks(1138) },
                    { 3L, new DateTime(2024, 9, 9, 11, 29, 47, 355, DateTimeKind.Utc).AddTicks(3693), "user3@example.com", "Bob", "Johnson", "$2a$11$boVigJ0YLgLVx83tWpcnFeK5E6Rn6bl5UImvPSsdApqhO4tekruFC", new DateTime(2024, 9, 9, 11, 29, 47, 355, DateTimeKind.Utc).AddTicks(3694) }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "TransactionTypeId", "UpdatedAt" },
                values: new object[,]
                {
                    { "direct-credit", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(7861), "Direct Credit", null, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(7862) },
                    { "good-life", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(907), "Good Life", null, "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(908) },
                    { "home", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(1044), "Home", null, "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(1045) },
                    { "personal", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(7214), "Personal", null, "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(7215) },
                    { "transfer", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(6866), "Transfer", null, "transfer", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(6866) },
                    { "transport", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(5621), "Transport", null, "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(5622) },
                    { "adult", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(5316), "Adult", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(5317) },
                    { "booze", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(3039), "Booze", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(3039) },
                    { "car-insurance-and-maintenance", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(8943), "Car Insurance, Rego & Maintenance", "transport", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(8943) },
                    { "car-repayments", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(1992), "Repayments", "transport", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(1992) },
                    { "clothing-and-accessories", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(4100), "Clothing & Accessories", "personal", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(4101) },
                    { "cycling", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(5039), "Cycling", "transport", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(5040) },
                    { "education-and-student-loans", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(8461), "Education & Student Loans", "personal", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(8461) },
                    { "events-and-gigs", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(9381), "Events & Gigs", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(9382) },
                    { "family", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(9963), "Children & Family", "personal", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(9964) },
                    { "fitness-and-wellbeing", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(2531), "Fitness & Wellbeing", "personal", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(2531) },
                    { "fuel", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(269), "Fuel", "transport", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(270) },
                    { "games-and-software", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(7844), "Apps, Games & Software", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 981, DateTimeKind.Utc).AddTicks(7845) },
                    { "gifts-and-charity", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(6379), "Gifts & Charity", "personal", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(6380) },
                    { "groceries", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(2003), "Groceries", "home", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(2004) },
                    { "hair-and-beauty", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(9641), "Hair & Beauty", "personal", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(9642) },
                    { "health-and-medical", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(2691), "Health & Medical", "personal", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(2692) },
                    { "hobbies", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(3289), "Hobbies", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(3290) },
                    { "holidays-and-travel", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(7251), "Holidays & Travel", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(7252) },
                    { "home-insurance-and-rates", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(1261), "Rates & Insurance", "home", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(1261) },
                    { "home-maintenance-and-improvements", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(4038), "Maintenance & Improvements", "home", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(4039) },
                    { "homeware-and-appliances", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(6034), "Homeware & Appliances", "home", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 982, DateTimeKind.Utc).AddTicks(6035) },
                    { "internet", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(1804), "Internet", "home", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(1804) },
                    { "investments", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(5763), "Investments", "personal", "direct-credit", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(5764) },
                    { "life-admin", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(274), "Life Admin", "personal", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(275) },
                    { "lottery-and-gambling", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(478), "Lottery & Gambling", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(479) },
                    { "mobile-phone", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(2088), "Mobile Phone", "personal", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(2089) },
                    { "news-magazines-and-books", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(3677), "News, Magazines & Books", "personal", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(3678) },
                    { "parking", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(4745), "Parking", "transport", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(4746) },
                    { "pets", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(8051), "Pets", "home", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(8052) },
                    { "public-transport", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(8881), "Public Transport", "transport", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 983, DateTimeKind.Utc).AddTicks(8882) },
                    { "pubs-and-bars", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(3455), "Pubs & Bars", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(3456) },
                    { "rent-and-mortgage", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(4211), "Rent & Mortgage", "home", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(4211) },
                    { "restaurants-and-cafes", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(6567), "Restaurants & Cafes", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(6568) },
                    { "takeaway", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(1225), "Takeaway", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(1226) },
                    { "taxis-and-share-cars", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(4945), "Taxis & Share Cars", "transport", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(4946) },
                    { "technology", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(6073), "Technology", "personal", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(6073) },
                    { "tobacco-and-vaping", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(2893), "Tobacco & Vaping", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(2893) },
                    { "toll-roads", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(7740), "Tolls", "transport", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(7740) },
                    { "tv-and-music", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(4542), "TV, Music & Streaming", "good-life", "purchase", new DateTime(2024, 9, 9, 11, 29, 46, 985, DateTimeKind.Utc).AddTicks(4543) },
                    { "utilities", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(8693), "Utilities", "home", "payment", new DateTime(2024, 9, 9, 11, 29, 46, 984, DateTimeKind.Utc).AddTicks(8694) }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedAt", "Currency", "Date", "ModifiedDescription", "OriginalDescription", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 6L, 3000.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(3625), "AUD", new DateOnly(2024, 8, 10), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(3625), 1L },
                    { 7L, -500.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(4501), "AUD", new DateOnly(2024, 8, 11), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(4510), 1L },
                    { 16L, 3200.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(1407), "AUD", new DateOnly(2024, 8, 9), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(1407), 2L },
                    { 17L, -800.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(1943), "AUD", new DateOnly(2024, 8, 11), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(1943), 2L },
                    { 21L, -23.45m, "transport", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(4137), "AUD", new DateOnly(2024, 7, 29), "Uber Rides", "UBER *RIDES SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(4137), 3L },
                    { 26L, 2900.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(2025), "AUD", new DateOnly(2024, 8, 9), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(2026), 3L },
                    { 27L, -1000.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(3015), "AUD", new DateOnly(2024, 8, 10), null, "TRANSFER TO MORTGAGE ACCOUNT ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(3015), 3L },
                    { 33L, -150.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(6586), "AUD", new DateOnly(2024, 8, 17), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(6586), 1L },
                    { 36L, 600.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(9968), "AUD", new DateOnly(2024, 8, 16), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(9969), 2L },
                    { 37L, -250.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(1184), "AUD", new DateOnly(2024, 8, 17), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(1185), 2L },
                    { 39L, -1200.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(2653), "AUD", new DateOnly(2024, 8, 17), null, "BOQ HOME LOAN PAYMENT ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(2653), 3L },
                    { 40L, 500.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(3262), "AUD", new DateOnly(2024, 8, 19), null, "TRANSFER FROM INVESTMENT ACCOUNT ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(3263), 3L },
                    { 41L, -200.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(3732), "AUD", new DateOnly(2024, 8, 20), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(3733), 3L },
                    { 42L, 1500.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(4276), "AUD", new DateOnly(2024, 8, 21), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(4276), 3L },
                    { 45L, 1000.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(5831), "AUD", new DateOnly(2024, 8, 21), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(5831), 2L },
                    { 49L, -150.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(7871), "AUD", new DateOnly(2024, 8, 25), null, "TRANSFER TO FRIEND ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(7872), 1L },
                    { 51L, -200.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(9217), "AUD", new DateOnly(2024, 8, 24), null, "TRANSFER TO FAMILY ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(9218), 2L },
                    { 52L, -500.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(9735), "AUD", new DateOnly(2024, 8, 25), null, "TRANSFER TO SUPERANNUATION ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(9735), 2L },
                    { 53L, 300.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(246), "AUD", new DateOnly(2024, 8, 26), null, "TRANSFER FROM SAVINGS ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(246), 3L },
                    { 55L, 3200.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(1292), "AUD", new DateOnly(2024, 8, 28), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(1292), 1L },
                    { 57L, 500.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(2380), "AUD", new DateOnly(2024, 8, 30), "Bonus", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(2380), 1L },
                    { 59L, 3300.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(3368), "AUD", new DateOnly(2024, 8, 28), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(3369), 2L },
                    { 61L, 700.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(4542), "AUD", new DateOnly(2024, 8, 30), "Performance Bonus", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(4542), 2L },
                    { 63L, 3100.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(5576), "AUD", new DateOnly(2024, 8, 28), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(5576), 3L },
                    { 65L, 400.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(6641), "AUD", new DateOnly(2024, 8, 30), "Overtime Payment", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(6641), 3L },
                    { 67L, 3250.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(7662), "AUD", new DateOnly(2024, 9, 1), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(7662), 1L },
                    { 69L, 3400.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(9178), "AUD", new DateOnly(2024, 9, 1), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(9179), 2L },
                    { 71L, 3200.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(349), "AUD", new DateOnly(2024, 9, 1), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(350), 3L },
                    { 73L, -25.50m, "transport", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(1419), "AUD", new DateOnly(2024, 9, 3), "Uber Rides", "UBER *RIDES BRISBANE ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(1420), 1L },
                    { 75L, 3000.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(2566), "AUD", new DateOnly(2024, 9, 6), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(2566), 1L },
                    { 76L, -600.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(3111), "AUD", new DateOnly(2024, 9, 7), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(3112), 1L },
                    { 81L, -200.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(5790), "AUD", new DateOnly(2024, 9, 12), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(5790), 1L },
                    { 82L, 3200.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(7031), "AUD", new DateOnly(2024, 9, 13), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(7031), 1L },
                    { 88L, 3400.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(1494), "AUD", new DateOnly(2024, 9, 6), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(1495), 2L },
                    { 89L, -900.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(2072), "AUD", new DateOnly(2024, 9, 7), null, "TRANSFER TO SAVINGS ACCOUNT ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(2072), 2L },
                    { 93L, 800.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(4120), "AUD", new DateOnly(2024, 9, 11), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(4120), 2L },
                    { 94L, -300.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(4684), "AUD", new DateOnly(2024, 9, 12), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(4684), 2L },
                    { 95L, 3300.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(5803), "AUD", new DateOnly(2024, 9, 13), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(5804), 2L },
                    { 99L, -35.00m, "transport", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(7968), "AUD", new DateOnly(2024, 9, 3), "Uber Rides", "UBER *RIDES SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(7968), 3L },
                    { 101L, 3100.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(9321), "AUD", new DateOnly(2024, 9, 6), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(9321), 3L },
                    { 102L, -1200.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(9903), "AUD", new DateOnly(2024, 9, 7), null, "TRANSFER TO MORTGAGE ACCOUNT ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(9903), 3L },
                    { 107L, -250.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(2502), "AUD", new DateOnly(2024, 9, 12), null, "TRANSFER TO CREDIT CARD ;", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(2502), 3L },
                    { 108L, 1700.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(3068), "AUD", new DateOnly(2024, 9, 13), "Freelance Work", "PAYMENT RECEIVED FROM CLIENT ;", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(3068), 3L },
                    { 110L, 3200.00m, "direct-credit", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(4153), "AUD", new DateOnly(2024, 9, 15), "Salary", "PAYMENT RECEIVED FROM EMPLOYER ;", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(4153), 3L },
                    { 111L, -1300.00m, "transfer", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(4682), "AUD", new DateOnly(2024, 9, 16), null, "BOQ HOME LOAN PAYMENT ;", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(4683), 3L },
                    { 1L, -45.60m, "restaurants-and-cafes", new DateTime(2024, 9, 9, 11, 29, 47, 581, DateTimeKind.Utc).AddTicks(9222), "AUD", new DateOnly(2024, 7, 31), "Uber Eats Delivery", "UBER *EATS HELP.UBER.COM ;", new DateTime(2024, 9, 9, 11, 29, 47, 581, DateTimeKind.Utc).AddTicks(9222), 1L },
                    { 2L, -150.00m, "groceries", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(117), "AUD", new DateOnly(2024, 8, 2), "Woolworths Groceries", "WOOLWORTHS 1234 SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(118), 1L },
                    { 3L, -200.00m, "clothing-and-accessories", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(1226), "AUD", new DateOnly(2024, 8, 4), null, "MYER DEPARTMENT STORE SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(1227), 1L },
                    { 4L, -450.00m, "holidays-and-travel", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(2005), "AUD", new DateOnly(2024, 8, 6), "Flights to Melbourne", "FLIGHT CENTRE MELBOURNE ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(2005), 1L },
                    { 5L, -15.99m, "tv-and-music", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(2952), "AUD", new DateOnly(2024, 8, 7), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(2952), 1L },
                    { 8L, 200.00m, "gifts-and-charity", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(5153), "AUD", new DateOnly(2024, 8, 12), "Gift from Parents", "TRANSFER FROM PARENTS ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(5154), 1L },
                    { 9L, -75.50m, "home-maintenance-and-improvements", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(5992), "AUD", new DateOnly(2024, 8, 14), null, "BUNNINGS WAREHOUSE BRISBANE ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(5993), 1L },
                    { 10L, -120.00m, "technology", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(6922), "AUD", new DateOnly(2024, 8, 15), null, "AMAZON AU MELBOURNE ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(6923), 1L },
                    { 11L, -60.00m, "fuel", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(8065), "AUD", new DateOnly(2024, 7, 30), null, "SHELL SERVICE STATION PERTH ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(8065), 2L },
                    { 12L, -70.00m, "fuel", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(8922), "AUD", new DateOnly(2024, 8, 1), null, "BP AUSTRALIA PERTH ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(8922), 2L },
                    { 13L, -99.99m, "games-and-software", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(9496), "AUD", new DateOnly(2024, 8, 3), null, "APPLE.COM SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 582, DateTimeKind.Utc).AddTicks(9497), 2L },
                    { 14L, -250.00m, "groceries", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(161), "AUD", new DateOnly(2024, 8, 4), null, "WOOLWORTHS 5678 PERTH ;", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(162), 2L },
                    { 15L, -15.00m, "parking", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(827), "AUD", new DateOnly(2024, 8, 5), null, "CITY OF PERTH PARKING ;", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(828), 2L },
                    { 18L, 500.00m, "gifts-and-charity", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(2459), "AUD", new DateOnly(2024, 8, 12), "Loan from Sister", "TRANSFER FROM SISTER ;", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(2459), 2L },
                    { 19L, -120.00m, "fitness-and-wellbeing", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(3050), "AUD", new DateOnly(2024, 8, 13), null, "REBEL SPORT BRISBANE ;", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(3050), 2L },
                    { 20L, -45.00m, "mobile-phone", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(3586), "AUD", new DateOnly(2024, 8, 15), null, "OPTUS BILL PAYMENT ;", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(3586), 2L },
                    { 22L, -180.00m, "groceries", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(4660), "AUD", new DateOnly(2024, 7, 31), "Groceries", "COLES SUPERMARKET ADELAIDE ;", new DateTime(2024, 9, 9, 11, 29, 47, 583, DateTimeKind.Utc).AddTicks(4660), 3L },
                    { 23L, -500.00m, "technology", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(38), "AUD", new DateOnly(2024, 8, 2), null, "JB HI-FI MELBOURNE ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(39), 3L },
                    { 24L, -320.00m, "homeware-and-appliances", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(807), "AUD", new DateOnly(2024, 8, 5), null, "HARVEY NORMAN PERTH ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(808), 3L },
                    { 25L, -15.99m, "tv-and-music", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(1480), "AUD", new DateOnly(2024, 8, 7), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(1480), 3L },
                    { 28L, 100.00m, "gifts-and-charity", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(3523), "AUD", new DateOnly(2024, 8, 12), "Gift from Friend", "TRANSFER FROM FRIEND ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(3523), 3L },
                    { 29L, -40.00m, "clothing-and-accessories", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(4241), "AUD", new DateOnly(2024, 8, 13), null, "BIG W SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(4242), 3L },
                    { 30L, -60.00m, "booze", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(4947), "AUD", new DateOnly(2024, 8, 14), null, "DAN MURPHY'S BRISBANE ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(4948), 3L },
                    { 31L, -60.00m, "groceries", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(5514), "AUD", new DateOnly(2024, 8, 15), null, "WOOLWORTHS 9999 BRISBANE ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(5515), 1L },
                    { 32L, -25.00m, "parking", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(6015), "AUD", new DateOnly(2024, 8, 16), null, "CITY OF SYDNEY PARKING ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(6016), 1L },
                    { 34L, -70.00m, "utilities", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(7091), "AUD", new DateOnly(2024, 8, 18), null, "TELSTRA BILL PAYMENT ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(7091), 1L },
                    { 35L, -200.00m, "clothing-and-accessories", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(8387), "AUD", new DateOnly(2024, 8, 15), null, "WESTFIELD SHOPPING CENTRE PERTH ;", new DateTime(2024, 9, 9, 11, 29, 47, 584, DateTimeKind.Utc).AddTicks(8387), 2L },
                    { 38L, -15.00m, "restaurants-and-cafes", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(2022), "AUD", new DateOnly(2024, 8, 18), null, "SUBWAY FRANCHISE ADELAIDE ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(2022), 2L },
                    { 43L, -900.00m, "rent-and-mortgage", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(4775), "AUD", new DateOnly(2024, 8, 22), null, "AUSSIE HOME LOANS SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(4775), 1L },
                    { 44L, -8.00m, "restaurants-and-cafes", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(5319), "AUD", new DateOnly(2024, 8, 23), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(5319), 1L },
                    { 46L, -60.00m, "home-maintenance-and-improvements", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(6310), "AUD", new DateOnly(2024, 8, 22), null, "BUNNINGS WAREHOUSE PERTH ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(6311), 2L },
                    { 47L, -120.00m, "groceries", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(6838), "AUD", new DateOnly(2024, 8, 23), "Groceries", "COLES SUPERMARKET MELBOURNE ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(6838), 3L },
                    { 48L, -250.00m, "technology", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(7332), "AUD", new DateOnly(2024, 8, 24), null, "KOGAN.COM SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(7333), 3L },
                    { 50L, -60.00m, "restaurants-and-cafes", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(8616), "AUD", new DateOnly(2024, 8, 26), "Tickets", "MELBOURNE ZOO MELBOURNE ;", new DateTime(2024, 9, 9, 11, 29, 47, 585, DateTimeKind.Utc).AddTicks(8616), 1L },
                    { 54L, -100.00m, "health-and-medical", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(773), "AUD", new DateOnly(2024, 8, 27), null, "MEDIBANK PRIVATE SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(773), 3L },
                    { 56L, 150.00m, "gifts-and-charity", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(1807), "AUD", new DateOnly(2024, 8, 29), "Returned Item Refund", "REFUND FROM AMAZON ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(1807), 1L },
                    { 58L, 20.00m, "groceries", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(2906), "AUD", new DateOnly(2024, 8, 31), "Groceries Refund", "REFUND FROM WOOLWORTHS ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(2907), 1L },
                    { 60L, 10.00m, "games-and-software", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(4022), "AUD", new DateOnly(2024, 8, 29), "App Refund", "REFUND FROM APPLE ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(4023), 2L },
                    { 62L, 30.00m, "fuel", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(5074), "AUD", new DateOnly(2024, 8, 31), "Fuel Refund", "REFUND FROM BP ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(5074), 2L },
                    { 64L, 50.00m, "clothing-and-accessories", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(6100), "AUD", new DateOnly(2024, 8, 29), "Clothing Refund", "REFUND FROM BIG W ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(6100), 3L },
                    { 66L, 100.00m, "technology", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(7194), "AUD", new DateOnly(2024, 8, 31), "Technology Refund", "REFUND FROM JB HI-FI ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(7194), 3L },
                    { 68L, 15.99m, "tv-and-music", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(8501), "AUD", new DateOnly(2024, 9, 2), "Subscription Refund", "REFUND FROM NETFLIX ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(8502), 1L },
                    { 70L, 25.00m, "fuel", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(9747), "AUD", new DateOnly(2024, 9, 2), "Fuel Refund", "REFUND FROM SHELL ;", new DateTime(2024, 9, 9, 11, 29, 47, 586, DateTimeKind.Utc).AddTicks(9748), 2L },
                    { 72L, 15.00m, "groceries", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(872), "AUD", new DateOnly(2024, 9, 2), "Groceries Refund", "REFUND FROM COLES ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(872), 3L },
                    { 74L, -180.00m, "groceries", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(2006), "AUD", new DateOnly(2024, 9, 4), "Woolworths Groceries", "WOOLWORTHS 1234 SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(2006), 1L },
                    { 77L, -220.00m, "technology", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(3606), "AUD", new DateOnly(2024, 9, 8), null, "AMAZON AU SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(3606), 1L },
                    { 78L, -90.00m, "home-maintenance-and-improvements", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(4180), "AUD", new DateOnly(2024, 9, 9), null, "BUNNINGS WAREHOUSE BRISBANE ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(4180), 1L },
                    { 79L, -9.50m, "restaurants-and-cafes", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(4696), "AUD", new DateOnly(2024, 9, 10), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(4696), 1L },
                    { 80L, -85.00m, "utilities", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(5267), "AUD", new DateOnly(2024, 9, 11), null, "TELSTRA BILL PAYMENT ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(5267), 1L },
                    { 83L, -300.00m, "clothing-and-accessories", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(7578), "AUD", new DateOnly(2024, 9, 14), null, "MYER DEPARTMENT STORE SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(7578), 1L },
                    { 84L, -55.00m, "restaurants-and-cafes", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(8300), "AUD", new DateOnly(2024, 9, 15), null, "UBER *EATS SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(8300), 1L },
                    { 85L, -480.00m, "holidays-and-travel", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(9770), "AUD", new DateOnly(2024, 9, 16), "Flights to Sydney", "FLIGHT CENTRE SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 587, DateTimeKind.Utc).AddTicks(9771), 1L },
                    { 86L, -80.00m, "fuel", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(424), "AUD", new DateOnly(2024, 9, 3), null, "BP SERVICE STATION PERTH ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(425), 2L },
                    { 87L, -220.00m, "groceries", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(931), "AUD", new DateOnly(2024, 9, 4), null, "WOOLWORTHS 5678 PERTH ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(931), 2L },
                    { 90L, -99.99m, "games-and-software", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(2590), "AUD", new DateOnly(2024, 9, 8), null, "APPLE.COM SYDNEY ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(2590), 2L },
                    { 91L, -130.00m, "fitness-and-wellbeing", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(3119), "AUD", new DateOnly(2024, 9, 9), null, "REBEL SPORT BRISBANE ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(3120), 2L },
                    { 92L, -50.00m, "mobile-phone", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(3607), "AUD", new DateOnly(2024, 9, 10), null, "OPTUS BILL PAYMENT ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(3607), 2L },
                    { 96L, -75.00m, "home-maintenance-and-improvements", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(6407), "AUD", new DateOnly(2024, 9, 14), null, "BUNNINGS WAREHOUSE PERTH ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(6408), 2L },
                    { 97L, -20.00m, "restaurants-and-cafes", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(6915), "AUD", new DateOnly(2024, 9, 15), null, "SUBWAY FRANCHISE ADELAIDE ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(6916), 2L },
                    { 98L, -250.00m, "clothing-and-accessories", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(7456), "AUD", new DateOnly(2024, 9, 16), null, "WESTFIELD SHOPPING CENTRE PERTH ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(7456), 2L },
                    { 100L, -190.00m, "groceries", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(8781), "AUD", new DateOnly(2024, 9, 4), "Groceries", "COLES SUPERMARKET ADELAIDE ;", new DateTime(2024, 9, 9, 11, 29, 47, 588, DateTimeKind.Utc).AddTicks(8781), 3L },
                    { 103L, -550.00m, "technology", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(390), "AUD", new DateOnly(2024, 9, 8), null, "JB HI-FI MELBOURNE ;", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(390), 3L },
                    { 104L, -80.00m, "booze", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(917), "AUD", new DateOnly(2024, 9, 9), null, "DAN MURPHY'S BRISBANE ;", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(917), 3L },
                    { 105L, -15.99m, "tv-and-music", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(1439), "AUD", new DateOnly(2024, 9, 10), null, "NETFLIX.COM 800-123-4567 ;", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(1439), 3L },
                    { 106L, -350.00m, "homeware-and-appliances", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(2003), "AUD", new DateOnly(2024, 9, 11), null, "HARVEY NORMAN PERTH ;", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(2003), 3L },
                    { 109L, -10.00m, "restaurants-and-cafes", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(3604), "AUD", new DateOnly(2024, 9, 14), null, "BOOST JUICE BRISBANE ;", new DateTime(2024, 9, 9, 11, 29, 47, 589, DateTimeKind.Utc).AddTicks(3604), 3L }
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
