using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class purchased_goods
    {
        [Key]
        public int purchased_goods_id { get; set; }
        public string? username { get; set; }
        public string item_name { get; set; }
        public string item_description { get; set; }
        public CategoryType item_category { get; set; }
        public string? CustomCategoryName { get; set; }
        public int number_of_items { get; set; }
        public int price { get; set; }
        [DataType(DataType.Date)]
        public DateTime purchased_date { get; set; }

        public enum CategoryType
        {
            [Display(Name = "Clothes")]
            Clothes,

            [Display(Name = "Non Perishable Foods")]
            NonPerishableFoods,

            [Display(Name = "Custom")]
            Custom
        }
    }
}
