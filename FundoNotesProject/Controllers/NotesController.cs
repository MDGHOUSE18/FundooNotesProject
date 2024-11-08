using CommonLayer;
using CommonLayer.Request_Models;
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

            var notes = _notesManager.CreateNewNotes(notesModel, userId);

            if (notes !=null)
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
                Success=false,
                Message= "Notesnot created",
                Data = notes
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllNotes()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            var allNotes =  _notesManager.GetAllNotes(userId);

            if(allNotes != null)
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
    }
}
