using _003_Notes_API_Database_01.DTOs;
using _003_Notes_API_Database_01.Models;

namespace _003_Notes_API_Database_01.Services
{
    public interface INotesService
    {
        PaginatedResponse<Note> GetAll(GetNotesQuery query);
        Note? GetById(int id);
        Note Create(CreateNoteRequest request);
        Note? Update(int id, UpdateNoteRequest request);
        bool Delete(int id);
    }
}
