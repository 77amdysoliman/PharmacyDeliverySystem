using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pharmacy.infrastructuree.Migrations
{
    /// <inheritdoc />
    public partial class AddPrescriptionToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrescriptionImagePath",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrescriptionImagePath",
                table: "Orders");
        }
    }
}
