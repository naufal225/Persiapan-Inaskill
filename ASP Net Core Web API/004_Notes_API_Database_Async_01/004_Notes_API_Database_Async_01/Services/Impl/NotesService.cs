using _004_Notes_API_Database_Async_01.Data;
using _004_Notes_API_Database_Async_01.DTOs;
using _004_Notes_API_Database_Async_01.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace _004_Notes_API_Database_Async_01.Services.Impl
{
    public class NotesService : INotesService
    {
        private readonly AppDbContext _dbContext;
        
        public NotesService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResponse<Note>> GetAll(GetNotesQuery query)
        {
            IQueryable<Note> notesQuery = _dbContext.Notes;

            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                string keyword = query.Search.Trim().ToLower();

                notesQuery = notesQuery.Where(note =>
                    note.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    note.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                );
            }

            notesQuery = query.Sort.Trim().ToLower() switch
            {
                "asc" => notesQuery.OrderBy(note => note.CreatedAt),
                _ => notesQuery.OrderByDescending(note => note.CreatedAt)
            };

            int totalItems = await notesQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

            List<Note> notes = await notesQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            PaginatedResponse<Note> response = new PaginatedResponse<Note>
            {
                Items = notes,
                TotalItems = totalItems,
                Page = query.Page,
                TotalPages = totalPages,
            };

            return response;
        }

        public async Task<Note?> GetById(int id)
        {
            return await _dbContext.Notes.FirstOrDefaultAsync(note => note.Id == id);
        }

        public async Task<Note> Create(CreateNoteRequest request)
        {
            Note note = new Note
            {
                Title = request.Title.Trim(),
                Content = request.Content.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _dbContext.AddAsync(note);
            await _dbContext.SaveChangesAsync();

            return note;
        }

        public async Task<Note?> Update(int id, UpdateNoteRequest request)
        {
            Note? note = await _dbContext.Notes.FirstOrDefaultAsync(note => note.Id == id);
            if(note == null)
            {
                return null;
            }

            note.Title = request.Title.Trim();
            note.Content = request.Content.Trim();
            note.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return note;
        }

        public async Task<bool> Delete(int id)
        {
            Note? note = await _dbContext.Notes.FirstOrDefaultAsync(note => note.Id == id);
            if(note == null)
            {
                return false;
            }

            _dbContext.Remove(note);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
