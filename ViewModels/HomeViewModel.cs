using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Artwork> Artworks { get; set; }
        public IEnumerable<Artist> Artists { get; set; }
        public IEnumerable<Exhibition> Exhibitions { get; set; }
    }
}
