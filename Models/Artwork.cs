using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Artwork
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Medium is required")]
        public string Medium { get; set; }

        [Required(ErrorMessage = "Size is required")]
        public string Size { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; }

        public int ArtistId { get; set; }
        public Artist? Artist { get; set; }
    }

}
