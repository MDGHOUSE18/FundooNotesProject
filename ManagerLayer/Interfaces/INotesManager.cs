using CommonLayer.Request_Models;
using RepositoryLayer.Entities;

namespace ManagerLayer.Interfaces
{
    public interface INotesManager
    {
        NotesEntity CreateNewNotes(NotesModel notesModel, int userId);
        string DeleteNotes(int userId, int notesId);
        List<NotesEntity> GetAllNotes(int userId);
        string GetNotesById(int userId, string noteId);
        string UpdateNotes();
    }
}