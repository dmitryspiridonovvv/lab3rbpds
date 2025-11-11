namespace FitnessCenterLab3.Models;

public class Client
{
    public int ClientID { get; set; }
    public string LastName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = "M";
    public string Phone { get; set; } = default!;
    public string? Email { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    public ICollection<MembershipSale>? Sales { get; set; }
    public ICollection<Visit>? Visits { get; set; }
}
