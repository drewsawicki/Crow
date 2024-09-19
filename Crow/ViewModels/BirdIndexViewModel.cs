using Crow.Models;

namespace Crow.ViewModels
{
    public class BirdIndexViewModel
    {
        public IEnumerable<Bird> Birds { get; set; } = new List<Bird>();
        public List<int> UserBirdIds { get; set; } = new List<int>();
    }
}
