using Crow.Models;

namespace Crow.ViewModels
{
    public class BirdIndexViewModel
    {
        public PaginatedList<Bird> Birds { get; set; }
        public List<int> UserBirdIds { get; set; } = new List<int>();
    }
}
