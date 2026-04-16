using _002_Notes_App_03.DTOs;
using _002_Notes_App_03.Models;

namespace _002_Notes_App_03.Services
{
    public interface INotesService
    {
        public PagedResponse<Note> GetAll(GetNotesQuery query);
        public Note? GetById(int id);
        public Note Create(CreateNoteRequest request);
        public Note? Update(int id, UpdateNoteRequest request);
        public bool Delete(int id);
    }
}
