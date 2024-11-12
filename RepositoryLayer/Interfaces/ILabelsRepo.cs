using CommonLayer.Responses;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelsRepo
    {
        LabelEntity AddLabel(int userId, string labelName);
        bool DeleteLabel(int labelId);
        List<LabelEntity> GetLabels(int userId);
        bool IsLabelExists(string labelName);
        ResponseModel<LabelEntity> UpdateLabel(int labelId, string newName);
        LabelEntity AddLabelToNote(int labelId, int noteId);
    }
}