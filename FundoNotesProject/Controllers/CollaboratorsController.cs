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
        private readonly ILogger<CollaboratorsController> _logger;

        public CollaboratorsController(ICollaboratorsManager collaboratorsManager, IUserManager userManager, INotesManager notesManager, ILogger<CollaboratorsController> logger)
        {
            this._collaboratorsManager = collaboratorsManager;
            this._userManager = userManager;
            this._notesManager = notesManager;
            _logger = logger;
            _logger.LogInformation($"NotesController initialized at {DateTime.Now}");
        }


        [Authorize]
        [HttpPost]
        [Route("add")]
        public IActionResult AddCollaborator([FromBody] CollaboratorModel model)
        {
            _logger.LogInformation("Attempting to add collaborator with Email: {Email} to NotesId: {NotesId}", model.Email, model.NotesId);

            var userExist = _userManager.IsRegistered(model.Email);
            var notesExists = _notesManager.IsNotesExists(model.NotesId);

            if (userExist && notesExists)
            {
                var userId = int.Parse(User.Claims.First(x => x.Type == "UserId").Value);
                var collaborator = _collaboratorsManager.AddCollaborator(model, userId);
                _logger.LogInformation("Collaboration added successfully for UserId: {UserId}", userId);

                return Ok(new ResponseModel<CollaboratorEntity>
                {
                    Success = true,
                    Message = "Collaboration added successfully",
                    Data = collaborator
                });
            }
            else if (!userExist)
            {
                _logger.LogWarning("User not found with email: {Email}", model.Email);
                return NotFound(new ResponseModel<CollaboratorEntity>
                {
                    Success = false,
                    Message = $"User Not found with this email {model.Email}",
                    Data = null
                });
            }
            else
            {
                _logger.LogWarning("Note not found with ID: {NotesId}", model.NotesId);
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
            _logger.LogInformation("Retrieving all collaborators for UserId: {UserId}", userId);

            var collaborators = _collaboratorsManager.GetCollaborators(userId);

            if (collaborators != null)
            {
                _logger.LogInformation("Successfully retrieved collaborators for UserId: {UserId}", userId);
                return Ok(new ResponseModel<List<CollaboratorEntity>>
                {
                    Success = true,
                    Message = "Retrieve all collaborators",
                    Data = collaborators
                });
            }

            _logger.LogWarning("No collaborators found for UserId: {UserId}", userId);
            return NotFound(new ResponseModel<CollaboratorEntity>
            {
                Success = false,
                Message = "Collaborators not found",
                Data = null
            });
        }

        [Authorize, HttpDelete]
        [Route("delete")]
        public IActionResult DeleteCollaborator([FromBody] CollaboratorModel model)
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == "UserId").Value);
            _logger.LogInformation("Attempting to delete collaborator with Email: {Email} from NotesId: {NotesId}", model.Email, model.NotesId);

            var result = _collaboratorsManager.DeleteCollaborator(userId, model);

            if (result)
            {
                _logger.LogInformation("Successfully deleted collaborator with Email: {Email} from NotesId: {NotesId}", model.Email, model.NotesId);
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Collaborator removed successfully."
                });
            }

            _logger.LogWarning("Collaborator not found for deletion with Email: {Email} on NotesId: {NotesId}", model.Email, model.NotesId);
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Collaborator not found for the provided details."
            });
        }

    }
}
