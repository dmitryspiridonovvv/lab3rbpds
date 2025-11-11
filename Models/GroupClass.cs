namespace FitnessCenterLab3.Models;

public class GroupClass
{
    public int GroupClassID { get; set; }
    public string Title { get; set; } = default!;
    public string Schedule { get; set; } = default!;
    public int TrainerID { get; set; }
    public Trainer Trainer { get; set; } = default!;
    public ICollection<ClassSignup>? Signups { get; set; }
}
