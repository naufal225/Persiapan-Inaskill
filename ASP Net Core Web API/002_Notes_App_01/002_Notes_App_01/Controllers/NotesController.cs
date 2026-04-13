using _002_Notes_App_01.DTOs;
using _002_Notes_App_01.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _002_Notes_App_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private static List<Note> _notes = new List<Note>();
        private static int _nextId = 1;

        [HttpGet]
        public ActionResult<PagedResponse<Note>> GetAll([FromQuery] GetNotesQuery query)
        {
            if(query.Page < 1)
            {
                BadRequest("Page must be greater or equal to 1.");
            }

            if(query.PageSize < 1)
            {
                BadRequest("Page size must be grater or equal to 1.");
            }

            if(query.PageSize > 100)
            {
                BadRequest("Page size cannot be grater than 100.");
            }

            IEnumerable<Note> notesQuery = _notes;

            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                string keyword = query.Search.Trim().ToLower();

                notesQuery = notesQuery.Where(
                    note => note.Title.Contains(keyword) ||
                    note.Content.Contains(keyword)
                );
            }

            notesQuery = query.Sort.ToLower() switch
            {
                "asc" => notesQuery.OrderBy(note => note.CreatedAt),
                "desc" => notesQuery.OrderByDescending(note => note.CreatedAt),
                _ => notesQuery.OrderByDescending(note => note.CreatedAt)
            };

            int totalItems = notesQuery.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

            List<Note> notes = notesQuery.Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            PagedResponse<Note> response = new()
            {
                PageSize = query.PageSize,
                Items = notes,
                TotalItems = totalItems,
                Page = query.Page,
                TotalPage = totalPages
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<Note> GetById(int id)
        {
            Note? note = _notes.Find(note => note.Id == id);
            if (note == null)
            {
                return NotFound($"Note with id {id} was not found");
            }

            return Ok(note);
        }

        [HttpPost]
        public ActionResult<Note> Create(CreateNoteRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest("Title cannot be empty or whitespace");
            }

            if(string.IsNullOrEmpty(request.Content))
            {
                return BadRequest("Content cannot be empty or whitespace");
            }

            Note note = new()
            {
                Id = _nextId++,
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return Ok(note);
        }

        [HttpPut("{id}")]
        public ActionResult<Note> Update(int id, UpdateNoteRequest request)
        {
            Note? note = _notes.Find(note => note.Id == id);
            if (note == null)
            {
                return BadRequest($"Note with id {id} cannot be found");
            }

            if(string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest($"Title cannot be empty or whitespace");
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest($"Content cannot be empty or whitespace");
            }

            note.Title = request.Title;
            note.Content = request.Content;
            note.UpdatedAt = DateTime.UtcNow;

            return Ok(note);
        }


    }
}
