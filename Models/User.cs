using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DockerExam_UsersApp.Models
{
    public class User
    {
        [BsonId] 
        [BsonRepresentation(BsonType.String)] 
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}