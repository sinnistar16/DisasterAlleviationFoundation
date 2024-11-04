using System.ComponentModel.DataAnnotations;
using static DisasterAlleviationFoundation.Models.good_donations;

namespace DisasterAlleviationFoundation.Models
{
    public class monetary_donations
    {
        [Key]
        public int monetary_donations_id { get; set; }
        public string? username { get; set; }
        public int amount_donated { get; set; }
        [DataType(DataType.Date)]
        public DateTime donation_date { get; set; }
        public AnonymousType is_anonymous { get; set; }
        public string? donator { get; set; }

        public enum AnonymousType
        {
            [Display(Name = "Yes")]
            Yes,

            [Display(Name = "No")]
            No,
        }
    }
}
