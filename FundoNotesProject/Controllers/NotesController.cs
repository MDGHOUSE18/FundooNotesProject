using CommonLayer;
using CommonLayer.Request_Models;
using CommonLayer.Responses;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RepositoryLayer.Entities;
using System.Text;

namespace FundooNotesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesManager _notesManager;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<NotesController> _logger;

        public NotesController(INotesManager notesManager,IDistributedCache distributedCache, ILogger<NotesController> logger)
        {
            _notesManager = notesManager;
            _distributedCache = distributedCache;
            _logger = logger;
            _logger.LogInformation($"NotesController initialized at {DateTime.Now}");
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateNotes([FromBody] NotesModel notesModel)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            _logger.LogInformation($"Creating a note for User ID: {userId}");

            var notes = _notesManager.CreateNotes(notesModel, userId);

            if (notes != null)
            {
                _logger.LogInformation("Note created successfully.");
                return Ok(new ResponseModel<NotesEntity>
                {
                    Success = true,
                    Message = "Notes created successfully",
                    Data = notes
                });
            }

            _logger.LogWarning("Failed to create note.");
            return BadRequest(new ResponseModel<NotesEntity>
            {
                Success = false,
                Message = "Notes not created",
                Data = null
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllNotes()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            _logger.LogInformation($"Retrieving all notes for User ID: {userId}");

            var allNotes = _notesManager.GetAllNotes(userId);

            if (allNotes != null)
            {
                _logger.LogInformation("Successfully retrieved all notes.");
                return Ok(new ResponseModel<List<NotesResponse>>
                {
                    Success = true,
                    Message = "Retrieved all notes successfully",
                    Data = allNotes
                });
            }

            _logger.LogWarning("Failed to retrieve notes.");
            return BadRequest(new ResponseModel<List<NotesEntity>>
            {
                Success = false,
                Message = "Failed to retrieve notes",
                Data = null
            });
        }

        [Authorize, HttpGet]
        [Route("cache")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            _logger.LogInformation("Fetching notes from Redis cache.");
            var cacheKey = "NotesList";
            string serializedNotesList;
            var NotesList = new List<NotesResponse>();
            byte[] redisNotesList = await _distributedCache.GetAsync(cacheKey);

            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<NotesResponse>>(serializedNotesList);
                _logger.LogInformation("Notes retrieved from Redis cache.");
            }
            else
            {
                var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
                NotesList = _notesManager.GetAllNotes(userId);
                serializedNotesList = JsonConvert.SerializeObject(NotesList);
                var redisNoteList = Encoding.UTF8.GetBytes(serializedNotesList);

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));

                await _distributedCache.SetAsync(cacheKey, redisNoteList, options);
                _logger.LogInformation("Notes cached in Redis.");
            }

            return Ok(new ResponseModel<List<NotesResponse>>
            {
                Success = true,
                Message = NotesList.Any() ? "Retrieved all notes successfully" : "No notes available",
                Data = NotesList
            });
        }

        [Authorize]
        [HttpGet]
        [Route("{notesId}")]
        public IActionResult GetNotesById(int notesId)
        {
            _logger.LogInformation($"Fetching note with ID: {notesId}");
            var notes = _notesManager.GetNotesById(notesId);

            if (notes != null)
            {
                _logger.LogInformation("Note retrieved successfully.");
                return Ok(new ResponseModel<NotesEntity>
                {
                    Success = true,
                    Message = $"Retrieved note successfully with ID {notesId}",
                    Data = notes
                });
            }

            _logger.LogWarning($"No note found with ID: {notesId}");
            return BadRequest(new ResponseModel<NotesEntity>
            {
                Success = false,
                Message = $"Note not found with ID {notesId}",
                Data = null
            });
        }

        [Authorize]
        [HttpPut]
        [Route("{notesId}")]
        public IActionResult UpdateNotes(int notesId, [FromBody] UpdateNotesModel updateNotesModel)
        {
            if (updateNotesModel == null)
            {
                _logger.LogWarning("UpdateNotes: Invalid input data provided.");
                return BadRequest(new ResponseModel<NotesEntity>
                {
                    Success = false,
                    Message = "Invalid input data. Please provide valid note details.",
                    Data = null
                });
            }

            _logger.LogInformation($"Updating note with ID: {notesId}");
            var result = _notesManager.UpdateNotes(notesId, updateNotesModel);

            if (result != null)
            {
                _logger.LogInformation("Note updated successfully.");
                return Ok(new ResponseModel<NotesEntity>
                {
                    Success = true,
                    Message = "Note updated successfully",
                    Data = result
                });
            }

            _logger.LogWarning("Failed to update note.");
            return Ok(new ResponseModel<NotesEntity>
            {
                Success = false,
                Message = "Note not updated",
                Data = null
            });
        }

        [Authorize]
        [HttpDelete]
        [Route("{notesId}")]
        public IActionResult DeleteNotes(int notesId)
        {
            _logger.LogInformation($"Deleting note with ID: {notesId}");

            if (_notesManager.DeleteNotes(notesId))
            {
                _logger.LogInformation("Note deleted successfully.");
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = $"Note deleted successfully with ID {notesId}",
                    Data = null
                });
            }

            _logger.LogWarning($"Failed to delete note with ID: {notesId}");
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = $"Note not found with ID {notesId}",
                Data = null
            });
        }

        [Authorize, HttpPatch]
        [Route("{notesId}/colour")]
        public IActionResult Colour(int notesId, string newColour)
        {
            _logger.LogInformation($"Attempting to update color of note with ID: {notesId} to {newColour}");

            if (string.IsNullOrEmpty(newColour))
            {
                _logger.LogWarning("Invalid color input. Color is required.");
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid color input. Please provide a valid color.",
                    Data = null
                });
            }

            var result = _notesManager.UpdateNotesColour(notesId, newColour);

            if (result)
            {
                _logger.LogInformation($"Note color updated successfully for note ID: {notesId}");
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = $"Note color updated to {newColour}",
                    Data = null
                });
            }

            _logger.LogWarning($"Failed to update color for note ID: {notesId}");
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = "Failed to update the note color. Please check if the color is valid.",
                Data = null
            });
        }

        [Authorize, HttpPatch]
        [Route("{notesId}/Remainder")]
        public IActionResult Remainder(int notesId, DateTime remainder)
        {
            _logger.LogInformation($"Setting remainder for note ID: {notesId} to {remainder}");

            var result = _notesManager.UpdateNotesRemainder(notesId, remainder);

            if (result)
            {
                _logger.LogInformation($"Remainder set successfully for note ID: {notesId}");
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = $"Note remainder updated to {remainder}",
                    Data = null
                });
            }

            _logger.LogWarning($"Failed to set remainder for note ID: {notesId}");
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = "Failed to update the note remainder. Please check the input.",
                Data = null
            });
        }

        [Authorize, HttpPatch]
        [Route("{notesId}/togglePin")]
        public IActionResult TogglePin(int notesId)
        {
            _logger.LogInformation($"Toggling pin status for note ID: {notesId}");

            var result = _notesManager.TogglePinStatus(notesId);

            if (result)
            {
                _logger.LogInformation($"Note with ID: {notesId} pinned successfully.");
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Note pinned successfully.",
                    Data = null
                });
            }

            _logger.LogInformation($"Note with ID: {notesId} unpinned successfully.");
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Note unpinned successfully.",
                Data = null
            });
        }

        [Authorize, HttpPatch]
        [Route("{notesId}/toggleArchive")]
        public IActionResult ToggleArchive(int notesId)
        {
            _logger.LogInformation($"Toggling archive status for note ID: {notesId}");

            var result = _notesManager.ToggleArchiveStatus(notesId);

            if (result)
            {
                _logger.LogInformation($"Note with ID: {notesId} archived successfully.");
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Note archived successfully.",
                    Data = null
                });
            }

            _logger.LogInformation($"Note with ID: {notesId} unarchived successfully.");
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Note unarchived successfully.",
                Data = null
            });
        }

        [Authorize, HttpPatch]
        [Route("{notesId}/toggleTrash")]
        public IActionResult ToggleTrash(int notesId)
        {
            _logger.LogInformation($"Toggling trash status for note ID: {notesId}");

            var result = _notesManager.ToggleTrashStatus(notesId);

            if (result)
            {
                _logger.LogInformation($"Note with ID: {notesId} moved to trash successfully.");
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Note trashed successfully.",
                    Data = null
                });
            }

            _logger.LogInformation($"Note with ID: {notesId} restored from trash successfully.");
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Note untrashed successfully.",
                Data = null
            });
        }

    }
}
