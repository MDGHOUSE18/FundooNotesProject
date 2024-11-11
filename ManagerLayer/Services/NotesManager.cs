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


        public NotesEntity CreateNotes(NotesModel notesModel,int userId)
        {
            return _notesrepo.CreateNotes(notesModel,userId);
        }

        public bool IsNotesExists(int notesId)
        {
            return _notesrepo.IsNotesExists(notesId);
        }
        public List<NotesEntity> GetAllNotes(int userId)
        {
            return _notesrepo.GetAllNotes(userId);
        }

        public NotesEntity GetNotesById(int noteId)
        {
            return _notesrepo.GetNotesById(noteId);
        }


        public NotesEntity UpdateNotes(int notesId, UpdateNotesModel updateNotesModel)
        {
            return _notesrepo.UpdateNotes(notesId,updateNotesModel);
        }

        public bool DeleteNotes(int notesId)
        {
            return _notesrepo.DeleteNotes(notesId);
        }

        public bool UpdateNotesColour(int notesId,string newColour)
        {
            return _notesrepo.UpdateNotesColour(notesId,newColour);
        }

        public bool UpdateNotesRemainder(int notesId, DateTime? remainder)
        {
            return _notesrepo.UpdateNotesRemainder(notesId, remainder);
        }

        public bool ToggleArchiveStatus(int notesId)
        {
            return _notesrepo.ToggleArchiveStatus(notesId);
        }
        public bool TogglePinStatus(int notesId)
        {
            return _notesrepo.TogglePinStatus(notesId);
        }
        public bool ToggleTrashStatus(int notesId)
        {
            return _notesrepo.ToggleTrashStatus(notesId);
        }

    }
}
