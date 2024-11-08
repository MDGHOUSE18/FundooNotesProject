using CommonLayer.Request_Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
    public class NotesManager : INotesManager
    {
        private readonly INotesRepo _notesrepo;

        public NotesManager(INotesRepo notesrepo)
        {
            _notesrepo = notesrepo;
        }

        public NotesEntity CreateNewNotes(NotesModel notesModel,int userId)
        {
            return _notesrepo.CreateNotes(notesModel,userId);
        }

        public List<NotesEntity> GetAllNotes(int userId)
        {
            return _notesrepo.GetAllNotes(userId);
        }

        public string GetNotesById(int userId, string noteId)
        {
            return _notesrepo.GetNotesById();
        }


        public string UpdateNotes()
        {
            return _notesrepo.UpdateNotes();
        }

        public string DeleteNotes(int userId, int notesId)
        {
            return _notesrepo.DeleteNotes(userId, notesId);
        }
    }
}
