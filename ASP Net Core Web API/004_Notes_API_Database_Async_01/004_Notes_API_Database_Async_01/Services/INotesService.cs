using _004_Notes_API_Database_Async_01.DTOs;
using _004_Notes_API_Database_Async_01.Models;

namespace _004_Notes_API_Database_Async_01.Services
{
    public interface INotesService
    {
        Task<PaginatedResponse<Note>> GetAll(GetNotesQuery query);
        Task<Note?> GetById(int id);
        Task<Note> Create(CreateNoteRequest request);
        Task<Note?> Update(int id, UpdateNoteRequest request);
        Task<bool> Delete(int id);
    }
}
