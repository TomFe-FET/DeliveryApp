using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryAppDTO
{
public partial class OrderHeadDTO
{
    public String No { get; set; }
    public int DebNo { get; set; }
    public String DebName { get; set; }
    public String DebName2 { get; set; }
    public String Barcode { get; set; }
    public List<OrderLineDTO> OrderLines { get; set; }
}
}
