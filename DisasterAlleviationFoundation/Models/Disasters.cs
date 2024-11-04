using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class disasters
    {
        [Key]
        public int disaster_id { get; set; }
        public string? username { get; set; }
        public string disaster_location { get; set; }
        public string disaster_description { get; set; }
        public string disaster_aid_type { get; set; }
        [DataType(DataType.Date)]
        public DateTime disaster_start_date { get; set; }
        [DataType(DataType.Date)]
        public DateTime disaster_end_date { get; set; }


        //Handles the assigning of goods and  oney to a disaster
        public int? money_allocated_amount { get; set; }
        [DataType(DataType.Date)]
        public DateTime? money_allocated_date { get; set; }
        public string? good_allocated_name { get; set; }

        public string? username_money_allocated { get; set; }
        public int good_allocated_number_of_items { get; set; }
        [DataType(DataType.Date)]
        public DateTime? good_allocation_date { get; set; }
        public string? username_good_allocated { get; set; }

    }
}