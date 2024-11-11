using CommonLayer;
using CommonLayer.Request_Models;
using CommonLayer.Responses;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entities;

namespace FundooNotesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesManager _notesManager;

        public NotesController(INotesManager notesManager)
        {
            _notesManager = notesManager;
        }

        [Authorize]
        [HttpPost]
        [Route("createnote")]
        public IActionResult CreateNotes([FromBody] NotesModel notesModel)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var notes = _notesManager.CreateNotes(notesModel, userId);

            if (notes != null)
            {
                return Ok(new ResponseModel<NotesEntity>
                {
                    Success = true,
                    Message = "Notes created Succefully",
                    Data = notes
                });
            }
            return BadRequest(new ResponseModel<NotesEntity>
            {
                Success = false,
                Message = "Notesnot created",
                Data = notes
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllNotes()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            var allNotes = _notesManager.GetAllNotes(userId);

            if (allNotes != null)
            {
                return Ok(new ResponseModel<List<NotesEntity>>
                {
                    Success = true,
                    Message = "Retrieved all notes successfully",
                    Data = allNotes
                });
            }

            return BadRequest(new ResponseModel<List<NotesEntity>>
            {
                Success = false,
                Message = "Failed to retrieve notes",
                Data = null
            });
        }


        [Authorize]
        [HttpGet]
        [Route("{notesId}")]
        public IActionResult GetNotesById(int notesId)
        {
            var notes = _notesManager.GetNotesById(notesId);
            if (notes != null)
            {
                return Ok(new ResponseModel<NotesEntity>
                {
                    Success = true,
                    Message = $"Retrieve notes Successfully with this id {notesId}",
                    Data = notes
                });
            }
            return BadRequest(new ResponseModel<NotesEntity>
            {
                Success = false,
                Message = $"Notes not found with this id {notesId}",
                Data = null
            });
        }

        [Authorize]
        [HttpPut]
        [Route("update/{notesId}")]
        public IActionResult UpdateNotes(int notesId, [FromBody] UpdateNotesModel updateNotesModel)
        {
            if (updateNotesModel == null)
            {
                return BadRequest(new ResponseModel<NotesEntity>
                {
                    Success = false,
                    Message = "Invalid input data. Please provide valid note details.",
                    Data = null
                });
            }

            var result = _notesManager.UpdateNotes(notesId, updateNotesModel);

            if (result != null)
            {
                return Ok(new ResponseModel<NotesEntity>
                {
                    Success = true,
                    Message = "Notes updated successfully",
                    Data = result
                });
            }

            return Ok(new ResponseModel<NotesEntity>
            {
                Success = false,
                Message = "Notes not updated",
                Data = null
            });
        }

        [Authorize]
        [HttpDelete]
        [Route("delete/{notesId}")]
        public IActionResult DeleteNotes(int notesId)
        {
            if (_notesManager.DeleteNotes(notesId))
            {
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = $"Notes deleted Successfully with this id {notesId}",
                    Data = null
                });
            }
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = $"Notes not found with this id {notesId}",
                Data = null
            });
        }

        [Authorize, HttpPatch]
        [Route("{notesId}/updateColour")]
        public IActionResult Colour(int notesId, string newColour)
        {
            if (string.IsNullOrEmpty(newColour))
            {
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
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = $"Note color updated to {newColour}",
                    Data = null
                });
            }
            else
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Failed to update the note color. Please check if the color is valid.",
                    Data = null
                });
            }

        }

        [Authorize, HttpPatch]
        [Route("{notesId}/Remainder")]
        public IActionResult Remainder(int notesId, DateTime remainder)
        {

            var result = _notesManager.UpdateNotesRemainder(notesId, remainder);

            if (result)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = $"Note remainder updated to {remainder}",
                    Data = null
                });
            }
            else
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Failed to update the note . Please check if the color is valid.",
                    Data = null
                });
            }

        }

        [Authorize, HttpPatch]
        [Route("{notesId}/togglePin")]
        public IActionResult TogglePin(int notesId)
        {
            var result = _notesManager.TogglePinStatus(notesId);

            if (result)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Note pinned successfully.",
                    Data = null
                });
            }
            else
            {
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Note unpinned successfully.",
                    Data = null
                });
            }
        }

        [Authorize, HttpPatch]
        [Route("{notesId}/toggleArchive")]
        public IActionResult ToggleArchive(int notesId)
        {
            var result = _notesManager.ToggleArchiveStatus(notesId);

            if (result)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Note archived successfully.",
                    Data = null
                });
            }
            else
            {
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Note unarchived successfully.",
                    Data = null
                });
            }
        }


        [Authorize, HttpPatch]
        [Route("{notesId}/toggleTrash")]
        public IActionResult ToggleTrash(int notesId)
        {
            var result = _notesManager.ToggleTrashStatus(notesId);

            if (result)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Note trashed successfully.",
                    Data = null
                });
            }
            else
            {
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Note untrashed successfully.",
                    Data = null
                });
            }
        }
    }
}
