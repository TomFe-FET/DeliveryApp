using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DBConnection;
using DBConnection.Models;
using DeliveryAppDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Function
{
    public class Function1
    {
        private readonly DeliveryContext _deliveryContext;

        public Function1(DeliveryContext deliveryContext) => _deliveryContext = deliveryContext;


        [FunctionName("postOrder")]
        public async Task postOrders(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string[] args = new string[32];
            log.LogInformation("Received new Delievery");
            if (req != null)
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                OrderHeadDTO orderHeadDTO = JsonConvert.DeserializeObject<OrderHeadDTO>(content);
                OrderHead orderHead;
                if (orderHeadDTO != null)
                {
                    string newBarcode = orderHeadDTO.Barcode.Substring(1, orderHeadDTO.Barcode.Length - 2);
                    orderHead = new OrderHead
                    {
                        DebName = orderHeadDTO.DebName,
                        DebName2 = orderHeadDTO.DebName2,
                        DebNo = orderHeadDTO.DebNo,
                        No = orderHeadDTO.No,
                        Barcode = newBarcode,
                        Updated = false
                    };
                    log.LogInformation("Saving OrderHead to database");
                    _deliveryContext.OrderHead.Add(orderHead);
                    await _deliveryContext.SaveChangesAsync();
                    foreach (OrderLineDTO orderLineDTO in orderHeadDTO.OrderLines)
                    {
                        OrderLine orderLine = new OrderLine
                        {
                            LinesID = orderLineDTO.LinesID,
                            Amount = orderLineDTO.Amount,
                            ArticleDescription = orderLineDTO.ArticleDescription,
                            ArticleDescription2 = orderLineDTO.ArticleDescription2,
                            ArticleDescription3 = orderLineDTO.ArticleDescription3,
                            ArticleNo = orderLineDTO.ArticleNo,
                            OrderHeadNo = orderHead.No,
                            OrderHead = orderHead
                        };
                        log.LogInformation("Saving OrderLines to database");
                        _deliveryContext.OrderLine.Add(orderLine);
                        await _deliveryContext.SaveChangesAsync();
                    }
                }
            }
        }
        [FunctionName("getUpdatedOrders")]
        public  string getUpdatedOrders(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            List<OrderLine> updatedLines = new List<OrderLine>();
            List<OrderHead> updatedHeads = new List<OrderHead>();
            try
            {
                updatedHeads =  _deliveryContext.OrderHead
                                .Where(oH => oH.Updated == true)
                                .ToList();
                if (updatedHeads.Any())
                {
                    foreach (OrderHead oH in updatedHeads)
                    {
                        //falls mehrere Zeilen zu einem Kopf bestehen müssen alle in die updatedLines aufgenommen werden
                        List<OrderLine> linesFromHead = getUpdatedLine(oH.ID);
                        foreach(OrderLine oL in linesFromHead)
                        {
                            updatedLines.Add(oL);
                            _deliveryContext.OrderLine.Remove(oL);
                        }
                        _deliveryContext.OrderHead.Remove(oH);
                    }
                    _deliveryContext.SaveChanges();
                    return JsonConvert.SerializeObject(updatedLines);
                    
                }
                return null;
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return null;
            }
        }

        private List<OrderLine> getUpdatedLine(int iD)
        {
            return _deliveryContext.OrderLine
                    .Where(oL => oL.OrderHead.ID == iD)
                    .ToList();
        }
    }
}

