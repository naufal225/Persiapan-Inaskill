using _004_Notes_API_Database_Async_01.DTOs;
using _004_Notes_API_Database_Async_01.Models;
using _004_Notes_API_Database_Async_01.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _004_Notes_API_Database_Async_01.Controllers
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
        public async Task<ActionResult<PaginatedResponse<Note>>> GetAll([FromQuery] GetNotesQuery query)
        {
            if(query.Page < 1)
            {
                return BadRequestError("Page cannot be less than 1.");
            }

            if(query.PageSize < 1)
            {
                return BadRequestError("Page size cannot be less than 1.");
            }

            if (query.PageSize > 100)
            {
                return BadRequestError("Page size cannot be more than 100.");
            }

            PaginatedResponse<Note> response = await _notesService.GetAll(query);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note?>> GetById(int id)
        {
            Note? note = await _notesService.GetById(id);
            if(note == null)
            {
                return NotFoundError($"Note with id {id} cannot be found.");
            }

            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult<Note>> Create(CreateNoteRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequestError("Title cannot be empty.");
            }

            if(string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequestError("Content cannot be empty.");
            }

            Note note = await _notesService.Create(request);

            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Note?>> Update(int id, UpdateNoteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequestError("Title cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequestError("Content cannot be empty.");
            }

            Note? note = await _notesService.Update(id, request);
            if(note == null)
            {
                return NotFoundError($"Note with id {id} cannot be found.");
            }

            return Ok(note);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool deleted = await _notesService.Delete(id);

            if(!deleted)
            {
                return NotFoundError($"Note with id {id} cannot be found");
            }

            return NoContent();
        }

        private BadRequestObjectResult BadRequestError(string message)
        {
            return BadRequest(new ErrorResponse
            {
                Message = message
            });
        }


        private NotFoundObjectResult NotFoundError(string message)
        {
            return NotFound(new ErrorResponse
            {
                Message = message
            });
        }
    }
}
