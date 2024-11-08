using CommonLayer.Request_Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class NotesRepo : INotesRepo
    {

        private readonly FundooDBContext _context;

        public NotesRepo(FundooDBContext context)
        {
            _context = context;
        }


        public NotesEntity CreateNotes(NotesModel notesModel, int userId)
        {
            NotesEntity notes = new NotesEntity();

            notes.Title = notesModel.Title;
            notes.Description = notesModel.Description;

            //notes.Description = null;
            //notes.Image = null;
            //notes.IsArchive = false;
            //notes.IsPin = false;
            //notes.IsTrash= false;

            notes.CreatedAt = DateTime.Now;
            notes.UpdatedAt = DateTime.Now;
            notes.UserId = userId;

            _context.Add(notes);
            _context.SaveChanges();

            return notes;
        }

        public List<NotesEntity> GetAllNotes(int userId)
        {
            return _context.Notes.Where(x => x.UserId == userId).ToList();

        }

        public string GetNotesById()
        {
            return null;
        }

        public string UpdateNotes()
        {
            return null;
        }
        public string DeleteNotes(int userId, int notesId)
        {
            return null;
        }

    }
}
