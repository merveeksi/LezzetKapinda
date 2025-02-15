namespace LezzetKapinda.Models;

public sealed record Testimonial
{
    public Testimonial(int id, string clientName, string profession, string comment, string imageUrl, int rating, DateTimeOffset date)
    {
        Id = id;
        ClientName = clientName;
        Profession = profession;
        Comment = comment;
        ImageUrl = imageUrl;
        Rating = rating;
        Date = date;
    }

    public Testimonial()
    {
        
    }

    public int Id { get; set; }
    public string ClientName { get; set; }
    public string Profession { get; set; }
    public string Comment { get; set; }
    public string ImageUrl { get; set; }
    public int Rating { get; set; }
    
    public DateTimeOffset Date { get; set; }
}
