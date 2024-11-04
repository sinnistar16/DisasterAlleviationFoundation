using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class funds
    {
        [Key]
        public int id { get; set; }
        public int total_funds { get; set; }
    }
}
