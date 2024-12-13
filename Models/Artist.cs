namespace WebApplication3.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public string UserId { get; set; }

        public ICollection<Artwork> Artworks { get; set; }

        public ICollection<Exhibition> Exhibitions { get; set; }
    }

}
