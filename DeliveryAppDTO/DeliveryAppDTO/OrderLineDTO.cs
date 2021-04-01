using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryAppDTO
{
    public partial class OrderLineDTO
    {
        public String LinesID { get; set; }
        public int ArticleNo { get; set; }
        public String ArticleDescription { get; set; }
        public String ArticleDescription2 { get; set; }
        public String ArticleDescription3 { get; set; }
        public int Amount { get; set; }
        public int ReceiptNo { get; set; }
    }
}
