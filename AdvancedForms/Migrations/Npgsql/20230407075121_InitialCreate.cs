using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedForms.Migrations.Npgsql
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Forms",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Name = table.Column<string>(type: "text", nullable: false),
					Description = table.Column<string>(type: "text", nullable: false),
					UseCodes = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Forms", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "PresetTemplate",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Description = table.Column<string>(type: "text", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PresetTemplate", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Presets",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Code = table.Column<string>(type: "text", nullable: true),
					FormId = table.Column<Guid>(type: "uuid", nullable: false),
					TemplateId = table.Column<Guid>(type: "uuid", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Presets", x => x.Id);
					table.ForeignKey(
						name: "FK_Presets_Forms_FormId",
						column: x => x.FormId,
						principalTable: "Forms",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Presets_PresetTemplate_TemplateId",
						column: x => x.TemplateId,
						principalTable: "PresetTemplate",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "PresetTemplateValue",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Key = table.Column<string>(type: "text", nullable: false),
					Value = table.Column<string>(type: "text", nullable: false),
					TemplateId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PresetTemplateValue", x => x.Id);
					table.ForeignKey(
						name: "FK_PresetTemplateValue_PresetTemplate_TemplateId",
						column: x => x.TemplateId,
						principalTable: "PresetTemplate",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "PresetValue",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Key = table.Column<string>(type: "text", nullable: false),
					Value = table.Column<string>(type: "text", nullable: false),
					PresetId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PresetValue", x => x.Id);
					table.ForeignKey(
						name: "FK_PresetValue_Presets_PresetId",
						column: x => x.PresetId,
						principalTable: "Presets",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Responses",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					PresetId = table.Column<Guid>(type: "uuid", nullable: false),
					FormId = table.Column<Guid>(type: "uuid", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Responses", x => x.Id);
					table.ForeignKey(
						name: "FK_Responses_Forms_FormId",
						column: x => x.FormId,
						principalTable: "Forms",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Responses_Presets_PresetId",
						column: x => x.PresetId,
						principalTable: "Presets",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ResponseValue",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Key = table.Column<string>(type: "text", nullable: false),
					Value = table.Column<string>(type: "text", nullable: false),
					ResponseId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ResponseValue", x => x.Id);
					table.ForeignKey(
						name: "FK_ResponseValue_Responses_ResponseId",
						column: x => x.ResponseId,
						principalTable: "Responses",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Presets_FormId",
				table: "Presets",
				column: "FormId");

			migrationBuilder.CreateIndex(
				name: "IX_Presets_TemplateId",
				table: "Presets",
				column: "TemplateId");

			migrationBuilder.CreateIndex(
				name: "IX_PresetTemplateValue_TemplateId",
				table: "PresetTemplateValue",
				column: "TemplateId");

			migrationBuilder.CreateIndex(
				name: "IX_PresetValue_PresetId",
				table: "PresetValue",
				column: "PresetId");

			migrationBuilder.CreateIndex(
				name: "IX_Responses_FormId",
				table: "Responses",
				column: "FormId");

			migrationBuilder.CreateIndex(
				name: "IX_Responses_PresetId",
				table: "Responses",
				column: "PresetId");

			migrationBuilder.CreateIndex(
				name: "IX_ResponseValue_ResponseId",
				table: "ResponseValue",
				column: "ResponseId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "PresetTemplateValue");

			migrationBuilder.DropTable(
				name: "PresetValue");

			migrationBuilder.DropTable(
				name: "ResponseValue");

			migrationBuilder.DropTable(
				name: "Responses");

			migrationBuilder.DropTable(
				name: "Presets");

			migrationBuilder.DropTable(
				name: "Forms");

			migrationBuilder.DropTable(
				name: "PresetTemplate");
		}
	}
}
