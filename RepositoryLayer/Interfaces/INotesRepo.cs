using CommonLayer.Request_Models;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRepo
    {
        NotesEntity CreateNotes(NotesModel notesModel, int userId);
        bool IsNotesExists(int notesId);
        bool DeleteNotes(int notesId);
        List<NotesEntity> GetAllNotes(int userId);
        NotesEntity GetNotesById(int noteId);
        string UpdateNotes();
    }
}