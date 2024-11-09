using CommonLayer.Request_Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class CollaboratorsRepo : ICollaboratorsRepo
    {
        private readonly FundooDBContext _context;

        public CollaboratorsRepo(FundooDBContext context)
        {
            _context = context;
        }

        public CollaboratorEntity AddCollaborator(AddCollaboratorModel collaboratorModel, int userId)
        {
            CollaboratorEntity collaburator = new CollaboratorEntity();

            collaburator.NotesId = collaboratorModel.NotesId;
            collaburator.UserId = userId;
            collaburator.Email = collaboratorModel.Email;

            _context.Add(collaburator);
            _context.SaveChanges();

            return collaburator;
        }

        public List<CollaboratorEntity> GetCollaborators(int userId)
        {
            return _context.Collaborators.Where(x => x.UserId == userId).ToList();
        }

    }
}
