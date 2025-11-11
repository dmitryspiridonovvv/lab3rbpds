using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterLab3.Models
{
    public class Visit
    {
        [Key]
        public int VisitID { get; set; }

        public int ClientID { get; set; }
        public int ZoneID { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.Now;

        // Навигационные свойства (если нужно)
        public Client? Client { get; set; }
        public Zone? Zone { get; set; }
    }
}
