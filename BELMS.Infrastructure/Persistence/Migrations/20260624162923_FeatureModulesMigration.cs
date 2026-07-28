using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BELMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FeatureModulesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "WorkflowTasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompletedByUserId",
                table: "WorkflowTasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "WorkflowInstances",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "WorkflowInstances",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "WorkflowInstances",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "WorkflowInstances",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "WorkflowInstances",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UploadedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowAttachments_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowAttachments_WorkflowTasks_WorkflowTaskId",
                        column: x => x.WorkflowTaskId,
                        principalTable: "WorkflowTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTasks_CompletedByUserId",
                table: "WorkflowTasks",
                column: "CompletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowAttachments_UploadedByUserId",
                table: "WorkflowAttachments",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowAttachments_WorkflowTaskId",
                table: "WorkflowAttachments",
                column: "WorkflowTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTasks_Users_CompletedByUserId",
                table: "WorkflowTasks",
                column: "CompletedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("""
                UPDATE "WorkflowInstances"
                SET "EntityId" = "EmployeeId",
                    "EntityType" = 'EmployeeOnboarding',
                    "StartedAt" = COALESCE("CreatedAt", NOW())
                WHERE "EmployeeId" IS NOT NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTasks_Users_CompletedByUserId",
                table: "WorkflowTasks");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "WorkflowAttachments");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowTasks_CompletedByUserId",
                table: "WorkflowTasks");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "WorkflowTasks");

            migrationBuilder.DropColumn(
                name: "CompletedByUserId",
                table: "WorkflowTasks");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "WorkflowInstances");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "WorkflowInstances");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "WorkflowInstances");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "WorkflowInstances");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "WorkflowInstances",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
