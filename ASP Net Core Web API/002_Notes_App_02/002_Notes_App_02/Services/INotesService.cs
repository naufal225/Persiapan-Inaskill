using _002_Notes_App_02.DTOs;
using _002_Notes_App_02.Models;

namespace _002_Notes_App_02.Services
{
    public interface INotesService
    {
        PagedResponse<Note> GetAll(GetNotesQuery query);
        Note? GetById(int id);
        Note Create(CreateNoteRequest request);
        Note? Update(int id, UpdateNoteRequest request);
        bool Delete(int id);
    }
}
