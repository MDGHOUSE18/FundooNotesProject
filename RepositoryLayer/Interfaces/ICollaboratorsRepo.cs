using CommonLayer.Request_Models;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface ICollaboratorsRepo
    {
        CollaboratorEntity AddCollaborator(AddCollaboratorModel collaboratorModel, int userId);
        List<CollaboratorEntity> GetCollaborators(int userId);
    }
}