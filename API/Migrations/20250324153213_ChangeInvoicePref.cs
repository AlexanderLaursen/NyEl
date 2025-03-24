using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInvoicePref : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumerInvoicePreferences_InvoiceNotificationPreferences_InvoiceNotificationPreferenceId",
                table: "ConsumerInvoicePreferences");

            migrationBuilder.DropTable(
                name: "InvoiceNotificationPreferences");

            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "InvoicePreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    InvoiceNotificationPreference = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePreferences", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumerInvoicePreferences_InvoicePreferences_InvoiceNotificationPreferenceId",
                table: "ConsumerInvoicePreferences",
                column: "InvoiceNotificationPreferenceId",
                principalTable: "InvoicePreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumerInvoicePreferences_InvoicePreferences_InvoiceNotificationPreferenceId",
                table: "ConsumerInvoicePreferences");

            migrationBuilder.DropTable(
                name: "InvoicePreferences");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Invoices");

            migrationBuilder.CreateTable(
                name: "InvoiceNotificationPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceNotificationPreferences", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumerInvoicePreferences_InvoiceNotificationPreferences_InvoiceNotificationPreferenceId",
                table: "ConsumerInvoicePreferences",
                column: "InvoiceNotificationPreferenceId",
                principalTable: "InvoiceNotificationPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
