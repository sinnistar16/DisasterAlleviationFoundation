using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class good_donations
    {
        [Key]
        public int good_donations_id { get; set; }
        public string? username { get; set; }
        public string item_name { get; set; }
        public string item_description { get; set; }
        public CategoryType item_category { get; set; }
        public string? CustomCategoryName { get; set; }
        public int number_of_items { get; set; }
        [DataType(DataType.Date)]
        public DateTime donation_date { get; set; }
        public AnonymousType is_anonymous { get; set; }
        public string? donator { get; set; }


        public enum CategoryType
        {
            [Display(Name = "Clothes")]
            Clothes,

            [Display(Name = "Non Perishable Foods")]
            NonPerishableFoods,

            [Display(Name = "Custom")]
            Custom
        }

        public enum AnonymousType
        {
            [Display(Name = "Yes")]
            Yes,

            [Display(Name = "No")]
            No, 
        }
    }
}
