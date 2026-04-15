using _002_Notes_App_01.DTOs;
using _002_Notes_App_01.Models;
using _002_Notes_App_01.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace _002_Notes_App_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public ActionResult<PagedResponse<Note>> GetAll([FromQuery] GetNotesQuery query)
        {
            if(query.Page < 1)
            {
                return BadRequestError("Page must be greater or equal to 1.");
            }

            if(query.PageSize < 1)
            {
                return BadRequestError("Page size must be grater or equal to 1.");
            }

            if(query.PageSize > 100)
            {
                return BadRequestError("Page size cannot be grater than 100.");
            }

            PagedResponse<Note> response = _noteService.GetAll(query);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<Note> GetById(int id)
        {
            Note? note = _noteService.GetById(id);
            if (note == null)
            {
                return NotFoundError($"Note with id {id} was not found");
            }

            return Ok(note);
        }

        [HttpPost]
        public ActionResult<Note> Create(CreateNoteRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequestError("Title cannot be empty or whitespace");
            }

            if(string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequestError("Content cannot be empty or whitespace");
            }

            Note note = _noteService.Create(request);

            return CreatedAtAction(nameof(GetById), new { id = note.Id}, note);
        }

        [HttpPut("{id}")]
        public ActionResult<Note> Update(int id, UpdateNoteRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequestError($"Title cannot be empty or whitespace");
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequestError($"Content cannot be empty or whitespace");
            }

            Note? note = _noteService.Update(id, request);
            if (note == null)
            {
                return NotFoundError($"Note with id {id} cannot be found");
            }

            return Ok(note);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool delete = _noteService.Delete(id);

            if(!delete)
            {
                return NotFoundError($"Note with id {id} not found.");
            }

            return NoContent();
        }

        private NotFoundObjectResult NotFoundError(string message)
        {
            return NotFound(new ErrorResponse
            {
                Message = message
            });
        }

        private BadRequestObjectResult BadRequestError(string message)
        {
            return BadRequest(new ErrorResponse
            {
                Message = message
            });
        }
    }
}
