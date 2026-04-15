using _002_Notes_App_01.DTOs;
using _002_Notes_App_01.Models;

namespace _002_Notes_App_01.Services
{
    public interface INoteService
    {
        PagedResponse<Note> GetAll(GetNotesQuery query);
        Note? GetById(int id);
        Note Create(CreateNoteRequest request);
        Note? Update(int id, UpdateNoteRequest request);
        bool Delete(int id);
    }
}
