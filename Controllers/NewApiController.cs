using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DockerExam_UsersApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DockerExam_UsersApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            const string connectionString = Environment.GetEnvironmentVariable("MONGO_HOST")!;

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("MyDatabase");

            var collection = database.GetCollection<User>("InfoDb");

            return Ok(await collection.Find(_ => true).ToListAsync());
        }

    }
}