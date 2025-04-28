using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class fixScheduleRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorConsultationStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PatientConsultationStatus",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "DoctorConsultationStatusId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PatientConsultationStatusId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DoctorConsultationStatusId",
                table: "Users",
                column: "DoctorConsultationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PatientConsultationStatusId",
                table: "Users",
                column: "PatientConsultationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_MedicalConsultations_DoctorConsultationStatusId",
                table: "Users",
                column: "DoctorConsultationStatusId",
                principalTable: "MedicalConsultations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_MedicalConsultations_PatientConsultationStatusId",
                table: "Users",
                column: "PatientConsultationStatusId",
                principalTable: "MedicalConsultations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_MedicalConsultations_DoctorConsultationStatusId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_MedicalConsultations_PatientConsultationStatusId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DoctorConsultationStatusId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PatientConsultationStatusId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DoctorConsultationStatusId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PatientConsultationStatusId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "DoctorConsultationStatus",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientConsultationStatus",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
