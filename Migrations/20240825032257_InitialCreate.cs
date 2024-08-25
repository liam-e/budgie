using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CategoryId = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "08fccd17-af1b-42a5-9bc9-8a6dd1cf1bf8", "Shopping" },
                    { "13272b56-8e40-4f15-b355-76143b272190", "Income" },
                    { "4e2fe63c-da15-4a26-8499-105a48b0c8bc", "Rent" },
                    { "60bda4c4-4d17-4a29-9fe9-0ede28cf3b2b", "Transport" },
                    { "9f380f71-db1c-4d9d-9f4c-91244511a8d3", "Groceries" },
                    { "a8fed1bd-4d3a-4df6-b6b4-1742069573f6", "Healthcare" },
                    { "a9e4ee9e-1297-409a-a730-a62a38584b1d", "Home Improvement" },
                    { "aa32b77d-87da-4c63-856b-6897094702e8", "Travel" },
                    { "bc109365-15a8-49ad-a017-7623c0370755", "Dining" },
                    { "cbdf0d2b-198f-48d2-9e90-4ea086b4610c", "Utilities" },
                    { "ceb6ee8e-f240-456b-b754-d58846ae7063", "Entertainment" },
                    { "dd067c60-4b41-44dd-8e61-9565b9edc975", "Fitness" },
                    { "e0a001e6-b0c9-4b28-b5c4-764a0f79d120", "Transfer" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash" },
                values: new object[] { "053aac4e-0ea9-4006-9c90-30ec34e93f52", "user@example.com", "$2a$11$5cx6Vsp6aROK8GL8M3buauSuHIQFhorIZr3C3HFmNOO1OQ/d2YaMa" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "Date", "Description", "Type", "UserId" },
                values: new object[,]
                {
                    { 1L, 85.50m, "9f380f71-db1c-4d9d-9f4c-91244511a8d3", new DateOnly(2024, 1, 1), "Coles Supermarket Southbank", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 2L, 120.75m, "cbdf0d2b-198f-48d2-9e90-4ea086b4610c", new DateOnly(2024, 1, 2), "Electricity Bill - AGL", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 3L, 6.20m, "bc109365-15a8-49ad-a017-7623c0370755", new DateOnly(2024, 1, 5), "Starbucks Queen St", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 4L, 50.00m, "60bda4c4-4d17-4a29-9fe9-0ede28cf3b2b", new DateOnly(2024, 1, 7), "Myki Top-Up", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 5L, 1500.00m, "4e2fe63c-da15-4a26-8499-105a48b0c8bc", new DateOnly(2024, 1, 10), "Rent - 123 Main St", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 6L, 45.00m, "a9e4ee9e-1297-409a-a730-a62a38584b1d", new DateOnly(2024, 1, 12), "Bunnings Hardware Capalaba", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 7L, 19.99m, "ceb6ee8e-f240-456b-b754-d58846ae7063", new DateOnly(2024, 1, 15), "Netflix Subscription", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 8L, 60.00m, "cbdf0d2b-198f-48d2-9e90-4ea086b4610c", new DateOnly(2024, 1, 18), "Optus Bill", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 9L, 75.30m, "60bda4c4-4d17-4a29-9fe9-0ede28cf3b2b", new DateOnly(2024, 1, 20), "BP Fuel Cannon Hill", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 10L, 25.50m, "08fccd17-af1b-42a5-9bc9-8a6dd1cf1bf8", new DateOnly(2024, 1, 22), "Kmart Purchase Carindale", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 11L, 450.00m, "aa32b77d-87da-4c63-856b-6897094702e8", new DateOnly(2024, 1, 25), "Qantas Flight QF123", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 12L, 92.45m, "9f380f71-db1c-4d9d-9f4c-91244511a8d3", new DateOnly(2024, 1, 27), "Woolworths Grocery Morningside", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 13L, 40.00m, "dd067c60-4b41-44dd-8e61-9565b9edc975", new DateOnly(2024, 1, 30), "Goodlife Gym Membership", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 14L, 32.80m, "bc109365-15a8-49ad-a017-7623c0370755", new DateOnly(2024, 2, 1), "Uber Eats Order", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 15L, 11.99m, "ceb6ee8e-f240-456b-b754-d58846ae7063", new DateOnly(2024, 2, 3), "Spotify Subscription", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 16L, 30.00m, "a8fed1bd-4d3a-4df6-b6b4-1742069573f6", new DateOnly(2024, 2, 5), "Medicare Bulk Billing", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 17L, 150.00m, "08fccd17-af1b-42a5-9bc9-8a6dd1cf1bf8", new DateOnly(2024, 2, 7), "JB Hi-Fi Electronics Purchase", 1, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 18L, 3000.00m, "13272b56-8e40-4f15-b355-76143b272190", new DateOnly(2024, 2, 8), "Salary Deposit", 0, "053aac4e-0ea9-4006-9c90-30ec34e93f52" },
                    { 19L, 500.00m, "e0a001e6-b0c9-4b28-b5c4-764a0f79d120", new DateOnly(2024, 2, 10), "Savings Transfer", 2, "053aac4e-0ea9-4006-9c90-30ec34e93f52" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);

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
                name: "Category");
        }
    }
}
