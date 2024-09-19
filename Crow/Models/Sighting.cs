using System.ComponentModel.DataAnnotations;

namespace Crow.Models
{
    public class Sighting
    {
        public int Id { get; set; }

        public int UserBirdId { get; set; }

        public UserBird UserBird { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:H:mm}", ApplyFormatInEditMode = true)]

        public DateTime? Time { get; set; }

        public string? Location { get; set; }

        public string? Notes { get; set; }

        public Sighting()
        {
            UserBirdId = 0;
        }
    }
}
