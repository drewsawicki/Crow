using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Crow.Models
{
    public class Bird
    {
        public int Id { get; set; }

        [DisplayName("Common Name")]
        public string? CommonName { get; set; }

        [DisplayName("Scientific Name")]
        public string? ScientificName { get; set; }

        public ICollection<UserBird> UserBirds { get; set; }
     
        public Bird()
        {
            
        }
    }
}
