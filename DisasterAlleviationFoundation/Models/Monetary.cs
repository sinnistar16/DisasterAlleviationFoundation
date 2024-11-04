using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class monetary
    {
        [Key]
        public int monetary_id { get; set; }
        public string? username { get; set; }
        public int amount_donated { get; set; }
        [DataType(DataType.Date)]
        public DateTime donation_date { get; set; }
        public AnonymousType is_anonymous { get; set; }
        public string? donator { get; set; }
        public int? total_amount { get; set; }

        public enum AnonymousType
        {
            [Display(Name = "Yes")]
            Yes,

            [Display(Name = "No")]
            No,
        }
    }
}
