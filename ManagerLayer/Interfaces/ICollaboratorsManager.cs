using CommonLayer.Request_Models;
using RepositoryLayer.Entities;

namespace ManagerLayer.Interfaces
{
    public interface ICollaboratorsManager
    {
        CollaboratorEntity AddCollaborator(AddCollaboratorModel collaboratorModel, int userId);
        List<CollaboratorEntity> GetCollaborators(int userId);
    }
}