using _003_Notes_API_Database_01.Data;
using _003_Notes_API_Database_01.DTOs;
using _003_Notes_API_Database_01.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace _003_Notes_API_Database_01.Services.Impl
{
    public class NotesServiceImpl : INotesService
    {
        private readonly AppDbContext _dbContext;

        public NotesServiceImpl(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public PaginatedResponse<Note> GetAll(GetNotesQuery query)
        {
            IQueryable<Note> notesQuery = _dbContext.Notes.AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                string keyword = query.Search.Trim().ToLower();

                notesQuery = notesQuery.Where(note =>
                    note.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    note.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                );
            }

            notesQuery = query.Sort switch
            {
                "asc" => notesQuery.OrderBy(note => note.CreatedAt),
                _ => notesQuery.OrderByDescending(note => note.CreatedAt)
            };

            int totalItems = notesQuery.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

            List<Note> notes = notesQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            PaginatedResponse<Note> response = new PaginatedResponse<Note>
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
            return _dbContext.Notes.FirstOrDefault(note => note.Id == id);
        }

        public Note Create(CreateNoteRequest request)
        {
            Note note = new Note
            {
                Title = request.Title.Trim(),
                Content = request.Content.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.Notes.Add(note);
            _dbContext.SaveChanges();

            return note;
        }

        public Note? Update(int id, UpdateNoteRequest request)
        {

        }


    }
}
