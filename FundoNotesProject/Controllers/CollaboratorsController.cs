using CommonLayer;
using CommonLayer.Request_Models;
using CommonLayer.Responses;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entities;
using RepositoryLayer.Migrations;

namespace FundooNotesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorsController : ControllerBase
    {
        private readonly ICollaboratorsManager _collaboratorsManager;
        private readonly IUserManager _userManager;
        private readonly INotesManager _notesManager;

        public CollaboratorsController(ICollaboratorsManager collaboratorsManager, IUserManager userManager, INotesManager notesManager)
        {
            this._collaboratorsManager = collaboratorsManager;
            this._userManager = userManager;
            this._notesManager = notesManager;
        }


        [Authorize]
        [HttpPost]
        [Route("add")]
        public IActionResult AddCollaborator([FromBody] CollaboratorModel model)
        {
            var userExist = _userManager.IsRegistered(model.Email);
            var notesExists = _notesManager.IsNotesExists(model.NotesId);
            if (userExist && notesExists)
            {
                var userId = int.Parse(User.Claims.First(x => x.Type == "UserId").Value);
                var collaburator = _collaboratorsManager.AddCollaborator(model, userId);
                return Ok(new ResponseModel<CollaboratorEntity>
                {
                    Success = true,
                    Message = "Collaboration added successfully",
                    Data = collaburator
                });
            }
            else if (!userExist)
            {
                return NotFound(new ResponseModel<CollaboratorEntity>
                {
                    Success = false,
                    Message = $"User Not found with this email {model.Email}",
                    Data = null
                });
            }
            else
            {
                return NotFound(new ResponseModel<CollaboratorEntity>
                {
                    Success = false,
                    Message = $"Notes Not found with this id {model.NotesId}",
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetCollaborators()
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == "UserId").Value);

            var collaborators = _collaboratorsManager.GetCollaborators(userId);

            if (collaborators != null)
            {
                return Ok(new ResponseModel<List<CollaboratorEntity>>
                {
                    Success = true,
                    Message = $"Retrive all collaborators",
                    Data = collaborators
                });
            }
            return NotFound(new ResponseModel<CollaboratorEntity>
            {
                Success = false,
                Message = $"Collaborators Not found",
                Data = null
            });
        }

        [Authorize,HttpDelete]
        [Route("delete")]
        public IActionResult DeleteCollaborator([FromBody] CollaboratorModel model)
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == "UserId").Value);
            var result = _collaboratorsManager.DeleteCollaborator(userId, model);

            if (result)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Collaborator removed successfully."
                });
            }

            return NotFound(new ResponseModel<string>
            {
                Success = true,
                Message = "Collaborator not found for the provided details."
            });
        }

    }
}
