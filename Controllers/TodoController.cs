using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient; // New package, instead of "System" (ADO.NET) now use "Microsoft"
// Command to instal above NuGet package: dotnet add package Microsoft.Data.SqlClient || run in CMD inside root folder where u have .csproj file

using AbhayMVCapp.Models;
using System.Collections.Generic;
using System.Reflection;

namespace AbhayMVCapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // gives /api/todo
    public class TodoController: ControllerBase
    {
        private readonly IConfiguration _config;
        public TodoController(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        // Creating a GET Request
        [HttpGet]
        public IEnumerable<Todo> GetTodos(){
            var todos = new List<Todo>();
            using( var conn = GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT Id, Title, IsDone FROM Todos", conn);
                using( var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) {
                        todos.Add(new Todo
                        {
                            Id = (int)reader["Id"],
                            Title = reader["Title"].ToString(),
                            IsDone = (bool)reader["IsDone"]
                        });
                    }
                }
            }
            return todos;
        }

        [HttpPost]
        public IActionResult CreateTodo([FromBody] Todo todo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Todos (Title,IsDone) VALUES (@title,@isDone)", conn);
                cmd.Parameters.AddWithValue("@title", todo.Title);
                cmd.Parameters.AddWithValue("@isDone", todo.IsDone);
                cmd.ExecuteNonQuery();
            }
            return Ok("Todo Created!");
        }

        //Update API
        [HttpPut]
        public IActionResult UpdateTodo([FromBody] Todo todo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Todos SET Title=@title, IsDone=@isDone WHERE Id=@id",conn);
                cmd.Parameters.AddWithValue("@id", todo.Id);
                cmd.Parameters.AddWithValue("@title", todo.Title);
                cmd.Parameters.AddWithValue("@isDone", todo.IsDone);
                cmd.ExecuteNonQuery();
            }
            return Ok("Todo Updated!");
        }

        //Delete API
        [HttpDelete]
        public IActionResult DeleteTodo([FromBody] Todo todo) {
            using ( var conn = GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Todos WHERE Id=@id",conn);
                cmd.Parameters.AddWithValue("@id", todo.Id);
                cmd.ExecuteNonQuery();
            }
            return Ok("Todo Deleted!");
        }
        
    }
}
