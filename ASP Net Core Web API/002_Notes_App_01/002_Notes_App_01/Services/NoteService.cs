using _002_Notes_App_01.DTOs;
using _002_Notes_App_01.Models;
using Microsoft.AspNetCore.Mvc;

namespace _002_Notes_App_01.Services
{
    public class NoteService : INoteService
    {
        private static readonly List<Note> _notes = new List<Note>();
        private static int _nextId = 1;

        public PagedResponse<Note> GetAll(GetNotesQuery query)
        {
            IEnumerable<Note> notesQuery = _notes;

            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                string keyword = query.Search.Trim().ToLower();

                notesQuery = notesQuery.Where(
                    note => note.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    note.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)
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

            List<Note> items = notesQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            PagedResponse<Note> response = new PagedResponse<Note>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPage = totalPages
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

            _notes.Add(note);
            return note;
        }

        public Note? Update(int id, UpdateNoteRequest request)
        {
            Note? note = _notes.FirstOrDefault(note => note.Id == id);
            if(note == null)
            {
                return note;
            }

            note.Title = request.Title.Trim();
            note.Content = request.Content.Trim();
            note.UpdatedAt = DateTime.UtcNow;

            return note;
        }

        public bool Delete(int id)
        {
            Note? note = _notes.FirstOrDefault(note => note.Id == id);
            if (note == null)
            {
                return false;
            }

            _notes.Remove(note);
            return true;
        }
    }
}
