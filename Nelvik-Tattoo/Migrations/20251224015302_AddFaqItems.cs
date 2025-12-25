using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nelvik_Tattoo.Migrations
{
    /// <inheritdoc />
    public partial class AddFaqItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FaqItems",
                columns: new[] { "Id", "Answer", "CreatedAt", "Question", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "The price of a tattoo varies depending on size, placement, and level of detail.", new DateTime(2025, 12, 24, 1, 53, 1, 743, DateTimeKind.Utc).AddTicks(267), "How much does a tattoo cost?", 1, null },
                    { 2, "A deposit of NOK 1000 is required before an appointment can be confirmed. This deposit is non-refundable and will be deducted from the total price of the tattoo.\n", new DateTime(2025, 12, 24, 1, 53, 1, 743, DateTimeKind.Utc).AddTicks(1062), "Do you require a deposit?", 2, null },
                    { 3, "Before your tattoo appointment, make sure you are well-rested and have eaten and hydrated properly. Feel free to bring something sugary in case you need to boost your blood sugar during the session.", new DateTime(2025, 12, 24, 1, 53, 1, 743, DateTimeKind.Utc).AddTicks(1064), "How should I prepare for my appointment?", 3, null },
                    { 4, "If your tattoo is covered with second skin, you can keep it on for up to three days. After removing it, gently wash the tattoo with a mild, fragrance-free soap and apply an unscented moisturizer.\n", new DateTime(2025, 12, 24, 1, 53, 1, 743, DateTimeKind.Utc).AddTicks(1065), "How do I take care of my tattoo?", 4, null },
                    { 5, "Yes. Consultations are free and can help you and your tattoo artist figure out the design, placement, and details.", new DateTime(2025, 12, 24, 1, 53, 1, 743, DateTimeKind.Utc).AddTicks(1066), "Can I book a consultation?", 5, null },
                    { 6, "If you’re unable to attend your appointment, please let us know as early as possible. If the appointment is canceled, the deposit will be forfeited. However, the appointment can be rescheduled if you notify us at least 48 hours in advance.", new DateTime(2025, 12, 24, 1, 53, 1, 743, DateTimeKind.Utc).AddTicks(1067), "I can’t make it to my appointment — what should I do?", 6, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FaqItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FaqItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FaqItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FaqItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FaqItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FaqItems",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
