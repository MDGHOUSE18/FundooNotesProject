using CommonLayer.Responses;
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
    public class LabelsManager : ILabelsManager
    {
        private readonly ILabelsRepo _repo;

        public LabelsManager(ILabelsRepo repo)
        {
            this._repo = repo;
        }

        public LabelEntity AddLabel(int userId, string labelName)
        {
            return _repo.AddLabel(userId, labelName);
        }

        public bool DeleteLabel(int labelId)
        {
            return _repo.DeleteLabel(labelId);
        }

        public List<LabelEntity> GetLabels(int userId)
        {
            return _repo.GetLabels(userId);
        }

        public bool IsLabelExists(string labelName)
        {
            return _repo.IsLabelExists(labelName);
        }

        public ResponseModel<LabelEntity> UpdateLabel(int labelId, string newName)
        {
            return _repo.UpdateLabel(labelId, newName);
        }
        public LabelEntity AddLabelToNote(int labelId, int noteId)
        {
            return _repo.AddLabelToNote(labelId, noteId);
        }
    }
}
