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

        public LabelsController(ILabelsManager labelsManager)
        {
            this._labelsManager = labelsManager;
        }
        
        [Authorize,HttpPost]
        [Route("addlabel")]
        public IActionResult AddLabel(string labelName)
        {
            var result = _labelsManager.IsLabelExists(labelName);
            
            if (result)
            {
                var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value);
                var label = _labelsManager.AddLabel(userId,labelName);
                return Ok(new ResponseModel<LabelEntity>
                {
                    Success = true,
                    Message = "Label created Successfully",
                    Data = label
                });
            }
            return BadRequest(new ResponseModel<LabelEntity>
            {
                Success = false,
                Message = "Label already exists",
                Data = null
            });
        }

        [Authorize,HttpGet]
        [Route("getlabels")]
        public IActionResult GetAllLabels()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value);
            var labels = _labelsManager.GetLabels(userId);
            if (labels != null)
            {
                
                return BadRequest(new ResponseModel<List<LabelEntity>>
                {
                    Success = true,
                    Message = "Retrieved all notes successfully",
                    Data = labels
                });
            }
            return BadRequest(new ResponseModel<List<LabelEntity>>
            {
                Success = false,
                Message = "Failed to retrieve notes",
                Data = null
            });
        }

        [Authorize,HttpPut]
        [Route("{labelId}/edit")]
        public IActionResult EditLabel(int labelId,string newName)
        {
            var result = _labelsManager.UpdateLabel(labelId,newName);

            return Ok(result);
        }

        [Authorize,HttpPut]
        [Route("{labelId}/delete")]
        public IActionResult DeleteLabel(int labelId)
        {
            var result = _labelsManager.DeleteLabel(labelId);

            if (result)
            {
                return Ok(new ResponseModel<List<LabelEntity>>
                {
                    Success = true,
                    Message = $"Label deleted successfully with {labelId}",
                    Data = null
                });
            }
            return BadRequest(new ResponseModel<List<LabelEntity>>
            {
                Success = false,
                Message = $"Label not found with {labelId}",
                Data = null
            });
        }

        [Authorize,HttpPatch]
        [Route("addlabeltonote")]
        public IActionResult AddLabelToNote(int labelId,int notesId)
        {
            var result = _labelsManager.AddLabelToNote(labelId,notesId);

            if (result == null)
            {
                return BadRequest(new ResponseModel<LabelEntity>
                {
                    Success = false,
                    Message = "Label or Note not found.",
                    Data = null
                });
            }
            return Ok(new ResponseModel<LabelEntity>
            {
                Success = true,
                Message = "Add Label To Note succesfully",
                Data = result
            });
        }
    }
}
