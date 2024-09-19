using System.Reflection.Metadata.Ecma335;
using System.ComponentModel.DataAnnotations;


namespace Crow.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string? Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [DataType(DataType.Time)] 
        public DateTime? Time { get; set; }

        public string? Notes { get; set; }

        public int UserBirdId { get; set; }
        public UserBird UserBird { get; set; } = null!;

        public Photo()
        {
            
        }
    }
}
