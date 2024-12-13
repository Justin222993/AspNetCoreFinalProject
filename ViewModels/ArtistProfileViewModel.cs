using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class ArtistProfileViewModel
    {
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public List<ArtworkViewModel> Artworks { get; set; }
        public List<ExhibitionViewModel> Exhibitions { get; set; }
    }

    public class ArtworkViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ExhibitionViewModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FloorPlanUrl { get; set; }
    }
}
