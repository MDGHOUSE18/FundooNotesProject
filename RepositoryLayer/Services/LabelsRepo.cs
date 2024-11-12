using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using CommonLayer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class LabelsRepo : ILabelsRepo
    {
        private readonly FundooDBContext _context;

        public LabelsRepo(FundooDBContext context)
        {
            _context = context;
        }

        public LabelEntity AddLabel(int userId, string labelName)
        {
            LabelEntity label = new LabelEntity();
            label.Name = labelName;
            label.userId = userId;

            _context.Labels.Add(label);
            _context.SaveChanges();

            return label;
        }
        public bool IsLabelExists(string labelName)
        {
            var label = _context.Labels.FirstOrDefault(x => x.Name == labelName);
            if (label == null)
            {
                return true;
            }
            return false;
        }
        public List<LabelEntity> GetLabels(int userId)
        {
            //var labels =  _context.Labels.Where(x => x.userId == userId).ToList();

            var labels = from label in _context.Labels
                         where label.userId == userId
                         select label;

            return labels.ToList();
        }

        public ResponseModel<LabelEntity> UpdateLabel(int labelId, string newName)
        {
            //LabelEntity label =  (from labels in _context.Labels
            //                     where labels.LabelId == labelId
            //                     select labels).SingleOrDefault();

            var label = _context.Labels.FirstOrDefault(x => x.LabelId == labelId);

            if (label.Name != newName)
            {
                label.Name = newName;
                label.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return new ResponseModel<LabelEntity>
                {
                    Success = true,
                    Message = "Label name updated successfully.",
                    Data = label
                };
            }

            return new ResponseModel<LabelEntity>
            {
                Success = false,
                Message = "Label name is already the same, no update needed.",
                Data = label
            };
        }

        public bool DeleteLabel(int labelId)
        {
            var label = _context.Labels.FirstOrDefault(x => x.LabelId == labelId);

            if (label == null)
            {
                return false;
            }

            _context.Labels.Remove(label);
            _context.SaveChanges();
            return true;
        }

        public LabelEntity AddLabelToNote(int labelId, int noteId)
        {
            var label = _context.Labels.Find(labelId);
            var note = _context.Notes.Find(noteId);

            if (label == null || note == null)
            {
                return null;
            }

            note.Labels.Add(label);
            note.UpdatedAt = DateTime.Now;

            label.Notes.Add(note);
            label.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            
            return label;
        }

    }
}
