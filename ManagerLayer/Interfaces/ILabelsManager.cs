using CommonLayer.Responses;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface ILabelsManager
    {
        LabelEntity AddLabel(int userId, string labelName);
        bool DeleteLabel(int labelId);
        List<LabelEntity> GetLabels(int userId);
        bool IsLabelExists(string labelName);
        ResponseModel<LabelEntity> UpdateLabel(int labelId, string newName);
        LabelEntity AddLabelToNote(int labelId, int noteId);
    }
}
