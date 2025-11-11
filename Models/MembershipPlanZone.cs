namespace FitnessCenterLab3.Models;

public class MembershipPlanZone
{
    public int MembershipPlanID { get; set; }
    public MembershipPlan MembershipPlan { get; set; } = default!;
    public int ZoneID { get; set; }
    public Zone Zone { get; set; } = default!;
}
