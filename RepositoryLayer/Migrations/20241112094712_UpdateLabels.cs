using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    public partial class UpdateLabels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "LabelNotes",
            columns: table => new
            {
                LabelId = table.Column<int>(type: "int", nullable: false),
                NotesId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LabelNotes", x => new { x.LabelId, x.NotesId });
                table.ForeignKey(
                    name: "FK_LabelNotes_Labels_LabelId",
                    column: x => x.LabelId,
                    principalTable: "Labels",
                    principalColumn: "LabelId",
                    onDelete: ReferentialAction.NoAction);
                table.ForeignKey(
                    name: "FK_LabelNotes_Notes_NotesId",
                    column: x => x.NotesId,
                    principalTable: "Notes",
                    principalColumn: "NotesId",
                    onDelete: ReferentialAction.NoAction);
            });

                    migrationBuilder.CreateIndex(
                        name: "IX_LabelNotes_NotesId",
                        table: "LabelNotes",
                        column: "NotesId");

                    

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelEntityNotesEntity");
        }
    }
}
