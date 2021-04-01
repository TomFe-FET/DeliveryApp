using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DBConnection.Models
{
    public partial class OrderLine
    {
        [Required][Key]
        public int ID { get; set; }
        [Required]
        public String LinesID { get; set; }
        [Required]
        public int ArticleNo { get; set; }
        [Required]
        public String ArticleDescription { get; set; }
        public String ArticleDescription2 { get; set; }
        public String ArticleDescription3 { get; set; }
        [Required]
        public int Amount { get; set; }
        public String PictureURL { get; set; }
        public String ReceiptNo { get; set; }
        [Required]
        public String OrderHeadNo { get; set; }
        [Required]
        public OrderHead OrderHead { get; set; }

    }
}
