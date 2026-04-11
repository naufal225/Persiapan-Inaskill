using _001_Todo_App_01.Dtos;
using _001_Todo_App_01.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _001_Todo_App_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private static List<Todo> _todos = new List<Todo>();
        private static int _nextId = 1;

        [HttpGet]
        public ActionResult<List<Todo>> GetAll()
        {
            return Ok(_todos.OrderByDescending(todo => todo.CreatedAt).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Todo> GetById(int id)
        {
            Todo? todo = _todos.FirstOrDefault(todo => todo.Id == id);
            if (todo == null) return NotFound(new
            {
                message=$"Todo with id {id} was not found."
            });

            return Ok(todo);
        }

        [HttpPost]
        public ActionResult<Todo> Create(CreateTodoRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new
                {
                    message="Title is required."
                });
            }

            Todo todo = new Todo
            {
                Id = _nextId++,
                Title = request.Title.Trim(),
                IsDone = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _todos.Add(todo);

            return CreatedAtAction(nameof(GetById), new {id = todo.Id}, todo);
        }

        [HttpPut("{id}")]
        public ActionResult<Todo> Update(int id, UpdateTodoRequest request)
        {
            Todo? todo = _todos.FirstOrDefault(todo => todo.Id == id);
            if (todo == null) return NotFound(new
            {
                message = $"Todo with id {id} was not found."
            });

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new
                {
                    message = "Title is required."
                });
            }

            todo.Title = request.Title.Trim();
            todo.IsDone = request.IsDone;
            todo.UpdatedAt = DateTime.UtcNow;

            return Ok(todo);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Todo? todo = _todos.FirstOrDefault(todo => todo.Id == id);
            if (todo == null) return NotFound(new
            {
                message = $"Todo with id {id} was not found."
            });

            _todos.Remove(todo);

            return NoContent();
        }
    }
}
