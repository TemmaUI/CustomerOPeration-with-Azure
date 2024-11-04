 using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzureCustomerOPeration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLeadEntityIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Leads",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Leads",
                newName: "id");
        }
    }
}
