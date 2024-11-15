using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CommonLayer.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;
using RepositoryLayer.Entities;
using Microsoft.Data.SqlClient.DataClassification;
using CommonLayer.Request_Models;

namespace FundooNotesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {

        private readonly ILabelsManager _labelsManager;
        private readonly ILogger<LabelsController> _logger;

        public LabelsController(ILabelsManager labelsManager, ILogger<LabelsController> logger)
        {
            this._labelsManager = labelsManager;
            this._logger = logger;
            _logger.LogInformation($"LabelsController initialized at {DateTime.Now}");
        }

        [Authorize, HttpPost]
        public IActionResult AddLabel(string labelName)
        {
            _logger.LogInformation("Checking if label with name {LabelName} already exists", labelName);

            var labelExists = _labelsManager.IsLabelExists(labelName);

            if (!labelExists)
            {
                _logger.LogInformation("Creating a new label with name {LabelName}", labelName);
                var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value);
                var label = _labelsManager.AddLabel(userId, labelName);

                return Ok(new ResponseModel<LabelEntity>
                {
                    Success = true,
                    Message = "Label created successfully",
                    Data = label
                });
            }

            _logger.LogWarning("Label already exists with name {LabelName}", labelName);
            return BadRequest(new ResponseModel<LabelEntity>
            {
                Success = false,
                Message = "Label already exists",
                Data = null
            });
        }

        [Authorize, HttpGet]
        public IActionResult GetAllLabels()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value);

            _logger.LogInformation("Retrieving all labels for user with ID {UserId}", userId);

            var labels = _labelsManager.GetLabels(userId);

            if (labels != null && labels.Any())
            {
                _logger.LogInformation("Retrieved {Count} labels for user with ID {UserId}", labels.Count, userId);
                return Ok(new ResponseModel<List<LabelEntity>>
                {
                    Success = true,
                    Message = "Retrieved all labels successfully",
                    Data = labels
                });
            }

            _logger.LogWarning("No labels found for user with ID {UserId}", userId);
            return BadRequest(new ResponseModel<List<LabelEntity>>
            {
                Success = false,
                Message = "No labels found",
                Data = null
            });
        }


        [Authorize, HttpPut]
        [Route("{labelId}")]
        public IActionResult EditLabel(int labelId, string newName)
        {
            _logger.LogInformation("Editing label with ID {LabelId}. New name: {NewName}", labelId, newName);

            var result = _labelsManager.UpdateLabel(labelId, newName);

            if (result != null)
            {
                _logger.LogInformation("Label with ID {LabelId} updated successfully to {NewName}", labelId, newName);
                return Ok(new Response
                {
                    Success = true,
                    Message = $"Label updated successfully to {newName}"
                });
            }

            _logger.LogWarning("Label with ID {LabelId} not found", labelId);
            return NotFound(new Response
            {
                Success = false,
                Message = $"Label not found with ID {labelId}"
            });
        }

        [Authorize, HttpDelete]
        [Route("{labelId}")]
        public IActionResult DeleteLabel(int labelId)
        {
            _logger.LogInformation("Attempting to delete label with ID {LabelId}", labelId);

            var result = _labelsManager.DeleteLabel(labelId);

            if (result)
            {
                _logger.LogInformation("Label with ID {LabelId} deleted successfully", labelId);
                return Ok(new Response
                {
                    Success = true,
                    Message = $"Label deleted successfully with ID {labelId}"
                });
            }

            _logger.LogWarning("Label with ID {LabelId} not found", labelId);
            return NotFound(new Response
            {
                Success = false,
                Message = $"Label not found with ID {labelId}"
            });
        }

        [Authorize, HttpPost]
        [Route("Notes/{notesId}/Labels/{labelId}")]
        public IActionResult AddLabelToNote(int labelId, int notesId)
        {
            _logger.LogInformation("Adding label with ID {LabelId} to note with ID {NotesId}", labelId, notesId);

            var result = _labelsManager.AddLabelToNote(labelId, notesId);

            if (result == null)
            {
                _logger.LogWarning("Failed to add label with ID {LabelId} to note with ID {NotesId}. Label or Note not found.", labelId, notesId);
                return NotFound(new ResponseModel<LabelEntity>
                {
                    Success = false,
                    Message = "Label or Note not found.",
                    Data = null
                });
            }

            _logger.LogInformation("Label with ID {LabelId} added to note with ID {NotesId} successfully", labelId, notesId);
            return Ok(new ResponseModel<LabelEntity>
            {
                Success = true,
                Message = "Label added to note successfully",
                Data = result
            });
        }

    }
}
