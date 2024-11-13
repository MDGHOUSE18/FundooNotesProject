using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    public partial class NoteLabelRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create Labels table
            migrationBuilder.CreateTable(
                name: "Labels",
                columns: table => new
                {
                    LabelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labels", x => x.LabelId);
                    table.ForeignKey(
                        name: "FK_Labels_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            // Create LabelEntityNotesEntity table (many-to-many relationship)
            migrationBuilder.CreateTable(
                name: "LabelEntityNotesEntity",
                columns: table => new
                {
                    LabelsLabelId = table.Column<int>(type: "int", nullable: false),
                    NotesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelEntityNotesEntity", x => new { x.LabelsLabelId, x.NotesId });
                    table.ForeignKey(
                        name: "FK_LabelEntityNotesEntity_Labels_LabelsLabelId",
                        column: x => x.LabelsLabelId,
                        principalTable: "Labels",
                        principalColumn: "LabelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelEntityNotesEntity_Notes_NotesId",
                        column: x => x.NotesId,
                        principalTable: "Notes",
                        principalColumn: "NotesId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create indexes for the join table
            migrationBuilder.CreateIndex(
                name: "IX_LabelEntityNotesEntity_NotesId",
                table: "LabelEntityNotesEntity",
                column: "NotesId");

            migrationBuilder.CreateIndex(
                name: "IX_Labels_userId",
                table: "Labels",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelEntityNotesEntity");

            migrationBuilder.DropTable(
                name: "Labels");
        }
    }
}
