using CommonLayer.Request_Models;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRepo
    {
        NotesEntity CreateNotes(NotesModel notesModel, int userId);
        string DeleteNotes(int userId, int notesId);
        List<NotesEntity> GetAllNotes(int userId);
        string GetNotesById();
        string UpdateNotes();
    }
}