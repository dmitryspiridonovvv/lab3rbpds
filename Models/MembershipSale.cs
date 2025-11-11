namespace FitnessCenterLab3.Models;

public class MembershipSale
{
    public int MembershipSaleID { get; set; }
    public int ClientID { get; set; }
    public Client Client { get; set; } = default!;
    public int MembershipPlanID { get; set; }
    public MembershipPlan MembershipPlan { get; set; } = default!;
    public DateTime SaleDate { get; set; } = DateTime.Now;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? RemainingVisits { get; set; }
}
