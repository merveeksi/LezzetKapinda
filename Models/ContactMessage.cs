using LezzetKapinda.Entities;

namespace LezzetKapinda.Models;

public sealed class ContactMessage : EntityBase<long>
{
    public ContactMessage(string name, string email, string subject, string message, DateTimeOffset sentDate)
    {
        Name = name;
        Email = email;
        Subject = subject;
        Message = message;
        SentDate = sentDate;
    }

    public ContactMessage()
    {

    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTimeOffset SentDate { get; set; }
}