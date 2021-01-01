using System.ComponentModel.DataAnnotations;
using DatabaseInteraction.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class UserModel : EntityBaseWithoutUserId
    {
        [BsonElement("username")]
        [Required]
        [MinLength(3)]
        public string Username { get; set; }

        [BsonElement("password")]
        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [BsonElement("email")] [EmailAddress] public string Email { get; set; }

        [BsonElement("firstname")]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [BsonElement("lastname")]
        [MaxLength(30)]
        public string Lastname { get; set; }

        [BsonElement("profilephoto")] public string ProfilePhoto { get; set; }
    }
}