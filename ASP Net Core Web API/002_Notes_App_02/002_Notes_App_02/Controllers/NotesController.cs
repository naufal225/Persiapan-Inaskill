using _002_Notes_App_02.DTOs;
using _002_Notes_App_02.Models;
using _002_Notes_App_02.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _002_Notes_App_02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;

        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpGet]
        public ActionResult<PagedResponse<Note>> GetAll([FromQuery] GetNotesQuery query)
        {
            if (query.Page < 1)
            {
                return BadRequestError("Page cannot be less than 1.");
            }

            if (query.PageSize < 1)
            {
                return BadRequestError("Page size cannot be less than 1.");
            }

            if (query.PageSize > 100)
            {
                return BadRequestError("Page size cannot be more than 100.");
            }

            PagedResponse<Note> response = _notesService.GetAll(query);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<Note?> GetById(int id)
        {
            Note? note = _notesService.GetById(id);

            if(note == null)
            {
                return NotFoundError($"Note with id {id} not found");
            }

            return Ok(note);
        }

        [HttpPost]
        public ActionResult<Note> Create(CreateNoteRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequestError("Title cannot be empty");
            }

            if(string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequestError("Content cannot be empty");
            }

            Note note = _notesService.Create(request);

            return note;
        }

        [HttpPut("{id}")]
        public ActionResult<Note?> Update(int id, UpdateNoteRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequestError("Title cannot be empty");
            }

            if(string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequestError("Content cannot be empty");
            }

            Note? note = _notesService.Update(id, request);

            if (note == null) return NotFoundError($"Note with id {id} cannot be found");

            return CreatedAtAction(nameof(GetById), new {id = note.Id}, note);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool delete = _notesService.Delete(id);

            if(!delete)
            {
                return NotFoundError($"Note with id {id} cannot be found");
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
