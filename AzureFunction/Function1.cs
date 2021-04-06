using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DBConnection;
using DBConnection.Models;
using DeliveryAppDTO;

namespace AzureFunction
{
    public class Function1
    {
        private readonly DeliveryContext _deliveryContext;

        public Function1(DeliveryContext deliveryContext) => _deliveryContext = deliveryContext;


        [FunctionName("BCToSQL")]
        public async Task RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Received new Delievery");
            if (req != null)
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                OrderHeadDTO orderHeadDTO = JsonConvert.DeserializeObject<OrderHeadDTO>(content);
                OrderHead orderHead;
                if (orderHeadDTO != null)
                {
                    if (_deliveryContext.OrderHead.Find(orderHeadDTO.No) == null)
                    {
                        orderHead = new OrderHead
                        {
                            DebName = orderHeadDTO.DebName,
                            DebName2 = orderHeadDTO.DebName2,
                            DebNo = orderHeadDTO.DebNo,
                            No = orderHeadDTO.No,
                            Barcode = orderHeadDTO.Barcode
                        };
                    } else
                    {
                        orderHead = _deliveryContext.OrderHead.Find(orderHeadDTO.No);
                    }
                    foreach(OrderLineDTO orderLineDTO in orderHeadDTO.OrderLines)
                    {
                        OrderLine orderLine = new OrderLine { LinesID = orderLineDTO.LinesID, 
                                                                Amount = orderLineDTO.Amount,
                                                                ArticleDescription = orderLineDTO.ArticleDescription, 
                                                                ArticleDescription2 = orderLineDTO.ArticleDescription2,
                                                                ArticleDescription3 = orderLineDTO.ArticleDescription3, 
                                                                ArticleNo = orderLineDTO.ArticleNo,
                                                                OrderHeadNo = orderHead.No,
                                                                OrderHead = orderHead };

                        log.LogInformation("Saving OrderLines to database");
                        using (var db = _deliveryContext)
                        {
                            db.OrderLine.Add(orderLine);
                            await db.SaveChangesAsync();
                        }
                        orderHead.OrderLines.Add(orderLine);
                    }
                    log.LogInformation("Saving OrderHead to database");
                    using (var db = _deliveryContext)
                    {
                        db.OrderHead.Add(orderHead);
                        await db.SaveChangesAsync();
                    }
                }
            }
            


        }
    }
}
