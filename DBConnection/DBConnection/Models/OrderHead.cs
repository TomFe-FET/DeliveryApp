using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DBConnection.Models
{
    public partial class OrderHead
    {
        [Required][Key]
        public int ID { get; set; }
        [Required]
        public String No { get; set; }
        [Required]
        public int DebNo { get; set; }
        [Required]
        public String DebName { get; set; }
        [Required]
        public String DebName2 { get; set; }
        [Required]
        public String Barcode { get; set; }
        public ICollection<OrderLine> OrderLines { get; set; }

    }
}
