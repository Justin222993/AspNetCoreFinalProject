namespace WebApplication3.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();
        public decimal TotalPrice { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool IsCart { get; set; } = true;
    }

}
