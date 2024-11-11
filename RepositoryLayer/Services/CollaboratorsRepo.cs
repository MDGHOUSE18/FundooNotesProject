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

        public CollaboratorEntity AddCollaborator(CollaboratorModel collaboratorModel, int userId)
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

        public bool DeleteCollaborator(int userId, CollaboratorModel collaborator)
        {
            var collborator = _context.Collaborators.FirstOrDefault(
                x => x.UserId == userId && x.Email ==collaborator.Email && x.NotesId == collaborator.NotesId);

            if (collborator == null)
            {
                return false;
            }
            _context.Collaborators.Remove(collborator);

            _context.SaveChanges();
            return true;
        }
    }
}
