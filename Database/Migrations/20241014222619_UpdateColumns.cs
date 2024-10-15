using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Database.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class UpdateColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Prescriptions_PrescriptionId",
                table: "Medicines");

            migrationBuilder.RenameColumn(
                name: "PrescriptionId",
                table: "Medicines",
                newName: "PrescriptionModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Medicines_PrescriptionId",
                table: "Medicines",
                newName: "IX_Medicines_PrescriptionModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Prescriptions_PrescriptionModelId",
                table: "Medicines",
                column: "PrescriptionModelId",
                principalTable: "Prescriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Prescriptions_PrescriptionModelId",
                table: "Medicines");

            migrationBuilder.RenameColumn(
                name: "PrescriptionModelId",
                table: "Medicines",
                newName: "PrescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Medicines_PrescriptionModelId",
                table: "Medicines",
                newName: "IX_Medicines_PrescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Prescriptions_PrescriptionId",
                table: "Medicines",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
