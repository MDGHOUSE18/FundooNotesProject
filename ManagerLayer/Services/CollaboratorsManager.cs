using CommonLayer.Request_Models;
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
    public class CollaboratorsManager : ICollaboratorsManager
    {

        private readonly ICollaboratorsRepo _collaboratorRepo;

        public CollaboratorsManager(ICollaboratorsRepo collaboratorRepo)
        {
            this._collaboratorRepo = collaboratorRepo;
        }

        public CollaboratorEntity AddCollaborator(AddCollaboratorModel collaboratorModel, int userId)
        {
            return _collaboratorRepo.AddCollaborator(collaboratorModel, userId);
        }

        public List<CollaboratorEntity> GetCollaborators(int userId)
        {
            return _collaboratorRepo.GetCollaborators(userId);
        }

    }
}
