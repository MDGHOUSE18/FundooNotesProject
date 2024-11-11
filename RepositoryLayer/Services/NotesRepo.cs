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
            if (notes != null)
            {
                return notes;
            }
            return null;
        }

        public NotesEntity UpdateNotes(int notesId, UpdateNotesModel updateNotesModel)
        {
            var notes = GetNotesById(notesId);
            if (notes != null && updateNotesModel != null)
            {
                bool isUpdated = false;

                // Update only if there are new values
                if (updateNotesModel.Title != null && notes.Title != updateNotesModel.Title)
                {
                    notes.Title = updateNotesModel.Title;
                    isUpdated = true;
                }
                if (updateNotesModel.Description != null && notes.Description != updateNotesModel.Description)
                {
                    notes.Description = updateNotesModel.Description;
                    isUpdated = true;
                }
                if (updateNotesModel.Image != null && notes.Image != updateNotesModel.Image)
                {
                    notes.Image = updateNotesModel.Image;
                    isUpdated = true;
                }


                if (isUpdated)
                {
                    notes.UpdatedAt = DateTime.Now;

                    _context.SaveChanges();
                }

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

        public bool UpdateNotesColour(int notesId, string newColour)
        {
            var notes = _context.Notes.FirstOrDefault(x => x.NotesId == notesId);

            if (notes.Colour != newColour)
            {
                notes.Colour = newColour;
                notes.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateNotesRemainder(int notesId, DateTime? remainder)
        {
            var notes = _context.Notes.FirstOrDefault(x => x.NotesId == notesId);
            if (notes.Remainder != remainder)
            {
                if (remainder == null || remainder == DateTime.MinValue)
                {
                    notes.Remainder = null;
                }
                else
                {
                    notes.Remainder = remainder;
                }
                notes.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool TogglePinStatus(int notesId)
        {
            var notes = _context.Notes.FirstOrDefault(x => x.NotesId == notesId);

            if (notes.IsPin == false && notes.IsArchive == true) notes.IsArchive = false;

            notes.IsPin = !notes.IsPin;
            _context.SaveChanges();

            return true;
        }
        public bool ToggleArchiveStatus(int notesId)
        {
            var notes = _context.Notes.FirstOrDefault(x => x.NotesId == notesId);

            if (notes.IsArchive == false && notes.IsPin == true) notes.IsPin = false;

            notes.IsArchive = !notes.IsArchive;
            _context.SaveChanges();

            return true;
        }

        public bool ToggleTrashStatus(int notesId)
        {
            var notes = _context.Notes.FirstOrDefault(x => x.NotesId == notesId);

            if (notes.IsTrash == false && notes.IsPin == true) notes.IsPin = false;

            notes.IsTrash = !notes.IsTrash;
            _context.SaveChanges();

            return true;
        }

    }
}
