using Crow.Models;
using System.ComponentModel.DataAnnotations;

namespace Crow.ViewModels
{
    public class PhotoUploadViewModel
    {
        // Represents the photo entity itself
        public Photo Photo { get; set; } = new Photo();

        // This property will be used to hold the uploaded file
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
