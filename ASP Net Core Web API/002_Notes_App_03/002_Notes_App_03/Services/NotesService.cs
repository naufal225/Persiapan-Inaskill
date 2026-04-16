using _002_Notes_App_03.DTOs;
using _002_Notes_App_03.Models;

namespace _002_Notes_App_03.Services
{
    public class NotesService : INotesService
    {
        private static readonly List<Note> _notes = new List<Note>();
        private int _nextId = 1;

        public PagedResponse<Note> GetAll(GetNotesQuery query)
        {
            IEnumerable<Note> _notesQuery = _notes;

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
               string keyword = query.Search.Trim().ToLower();

                _notesQuery = _notesQuery.Where(note =>
                     note.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                     note.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                 );
            }

            _notesQuery = query.Sort.ToLower() switch {
                "asc" => _notesQuery.OrderBy(note => note.CreatedAt),
                _ => _notesQuery.OrderByDescending(note => note.CreatedAt)
            };

            int totalItems = _notesQuery.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

            List<Note> notes = _notesQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            PagedResponse<Note> response = new PagedResponse<Note>
            {
                Items = notes,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            return response;
        }

        public Note? GetById(int id)
        {
            return _notes.FirstOrDefault(note => note.Id == id);
        }

        public Note Create(CreateNoteRequest request)
        {
            Note note = new Note
            {
                Id = _nextId++,
                Title = request.Title.Trim(),
                Content = request.Content.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return note;
        }

        public Note? Update(int id, UpdateNoteRequest request)
        {
            Note? note = GetById(id);
            if(note == null)
            {
                return null;
            }

            note.Title = request.Title.Trim();
            note.Content = request.Content.Trim();
            note.UpdatedAt = DateTime.UtcNow;

            return note;
        }

        public bool Delete(int id) {
            Note? note = GetById(id);
            if (note == null)
            {
                return false;
            }

            _notes.Remove(note);

            return true;
        }
    }
}
