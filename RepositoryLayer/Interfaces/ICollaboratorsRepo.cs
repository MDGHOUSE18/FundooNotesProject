using CommonLayer.Request_Models;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface ICollaboratorsRepo
    {
        CollaboratorEntity AddCollaborator(CollaboratorModel collaboratorModel, int userId);
        List<CollaboratorEntity> GetCollaborators(int userId);
        public bool DeleteCollaborator(int userId, CollaboratorModel collaborator);
    }
}