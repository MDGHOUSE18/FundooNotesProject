using CommonLayer.Request_Models;
using RepositoryLayer.Entities;

namespace ManagerLayer.Interfaces
{
    public interface INotesManager
    {
        NotesEntity CreateNewNotes(NotesModel notesModel, int userId);
        bool IsNotesExists(int notesId);
        bool DeleteNotes(int notesId);
        List<NotesEntity> GetAllNotes(int userId);
        NotesEntity GetNotesById(int noteId);
        string UpdateNotes();
    }
}