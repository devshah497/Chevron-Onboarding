using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class BUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "Deadline",
                table: "Courses",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "DateAssigned",
                table: "Courses",
                newName: "Category");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedOnUtc",
                table: "UserCourses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedOnUtc",
                table: "UserCourses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeadlineUtc",
                table: "UserCourses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "UserCourses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Courses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Courses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Domain",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoiningDate",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedOnUtc",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "CompletedOnUtc",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "DeadlineUtc",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Domain",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JoiningDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Courses",
                newName: "Deadline");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Courses",
                newName: "DateAssigned");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Courses",
                type: "INTEGER",
                nullable: true);
        }
    }
}
