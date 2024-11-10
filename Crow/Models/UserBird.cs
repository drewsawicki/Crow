using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;

namespace Crow.Models
{
    public class UserBird
    {
        public int Id { get; set; }

        [DisplayName("User ID")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;

        public int BirdId { get; set; }

        public bool Favorite { get; set; }

        public Bird Bird { get; set; } = null!;

        public ICollection<Photo> Photos { get; set; } = [];

        public UserBird()
        {
            UserId = "";
            BirdId = 0;
            Favorite = false;
        }
    }
}