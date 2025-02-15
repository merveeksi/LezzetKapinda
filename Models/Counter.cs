namespace LezzetKapinda.Models;

public sealed class Counter
{
    public Counter(int id, string title, string value, string icon)
    {
        Id = id;
        Title = title;
        Value = value;
        Icon = icon;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Value { get; set; }
    public string Icon { get; set; }
}