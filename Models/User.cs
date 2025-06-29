using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApiTest.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")] 
        public int Id { get; set; }

        [Required]
        [Column("username")]  
        public required string Username { get; set; } 

        [Required]
        [Column("password_hash")] 
        public required string PasswordHash { get; set; } 
    }
}