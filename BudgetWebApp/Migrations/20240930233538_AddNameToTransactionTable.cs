using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Transaction");
        }
    }
}
