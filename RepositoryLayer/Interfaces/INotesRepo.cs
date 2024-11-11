using CommonLayer.Request_Models;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRepo
    {
        NotesEntity CreateNotes(NotesModel notesModel, int userId);
        bool DeleteNotes(int notesId);
        List<NotesEntity> GetAllNotes(int userId);
        NotesEntity GetNotesById(int noteId);
        bool IsNotesExists(int notesId);
        bool ToggleArchiveStatus(int notesId);
        bool TogglePinStatus(int notesId);
        bool ToggleTrashStatus(int notesId);
        NotesEntity UpdateNotes(int notesId, UpdateNotesModel updateNotesModel);
        bool UpdateNotesColour(int notesId, string newColour);
        bool UpdateNotesRemainder(int notesId, DateTime? remainder);
    }
}