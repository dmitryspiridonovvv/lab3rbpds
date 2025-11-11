namespace FitnessCenterLab3.Models;

public class Zone
{
    public int ZoneID { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<MembershipPlanZone>? PlanZones { get; set; }
}
