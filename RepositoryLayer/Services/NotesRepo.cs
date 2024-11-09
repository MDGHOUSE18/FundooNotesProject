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

        public bool IsNotesExists(int notesId)
        {
            var notes = _context.Notes.FirstOrDefault(x => x.NotesId == notesId);
            if (notes != null)
            {
                return true;
            }
            return false;
        }

        public NotesEntity GetNotesById(int noteId)
        {
            var notes = _context.Notes.FirstOrDefault(x => x.NotesId == noteId);
            if (notes !=null)
            {
                return notes;
            }
            return null;
        }

        public NotesEntity UpdateNotes(int notesId,UpdateNotesModel updateNotesModel)
        {
            var notes = GetNotesById(notesId);
            if (notes != null && updateNotesModel != null)
            {
                // Update only if there are new values
                if (updateNotesModel.Title != null)
                {
                    notes.Title = updateNotesModel.Title;
                }
                if (updateNotesModel.Description != null)
                {
                    notes.Description = updateNotesModel.Description;
                }
                if (updateNotesModel.Remainder != DateTime.MinValue)
                {
                    notes.Remainder = updateNotesModel.Remainder;
                }
                if (updateNotesModel.Colour != null)
                {
                    notes.Colour = updateNotesModel.Colour;
                }
                if (updateNotesModel.Image != null)
                {
                    notes.Image = updateNotesModel.Image;
                }

                // Correct assignment for boolean fields
                notes.IsArchive = updateNotesModel.IsArchive;
                notes.IsPin = updateNotesModel.IsPin;
                notes.IsTrash = updateNotesModel.IsTrash;

                // Update timestamp
                notes.UpdatedAt = DateTime.Now;

                // Save changes to the database
                _context.SaveChanges();
                return notes;
            }
            return null;
        }
        public bool DeleteNotes(int notesId)
        {
            var notes = _context.Notes.FirstOrDefault(x => x.NotesId == notesId);
            if (notes != null)
            {
                _context.Notes.Remove(notes);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        string INotesRepo.UpdateNotes()
        {
            return null;
        }
    }
}
