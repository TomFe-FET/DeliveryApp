using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using DBConnection;
using DBConnection.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace PowerAppToOCR
{
    public class PowerAppToOCR
    {
        private readonly DeliveryContext _deliveryContext;

        public PowerAppToOCR(DeliveryContext deliveryContext) => _deliveryContext = deliveryContext;

        [FunctionName("PowerAppToOCR")]
        public void Run([BlobTrigger("bonstorage/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            string subname = name.Substring(name.Length - 2);
            if (subname.Equals("MC"))
            {
                name = name.Substring(0, name.Length - 2);
            }                
            OrderLine orderLine = null;
            OrderHead orderHead = null;
            if (!name.Equals(""))
            {
                try
                {
                    orderLine = _deliveryContext.OrderLine
                        .Where(ol => ol.OrderHeadNo.Equals(name))
                        .SingleOrDefault();
                    if (orderLine != null)
                    {
                        orderHead = _deliveryContext.OrderHead
                            .Where(oH => oH.No.Equals(name))
                            .SingleOrDefault();
                    }
                    string pictureURL = "https://deliveryappblobstorage.blob.core.windows.net/bonstorage/" + name + "?sv=2020-02-10&ss=bfqt&srt=sco&sp=rwdlacupx&se=2024-08-13T17:35:48Z&st=2021-04-12T09:35:48Z&spr=https&sig=O0YDUvYJvbdPM4a9Nrow1fn4rXiKVAUYXQcf6uoywgY%3D";
                    if (!(subname.Equals("MC")))
                    {
                        formrecognizer ocr = new formrecognizer();
                        ocr.Run(pictureURL);
                        if (ocr.newAmount.Equals(""))
                        {
                            orderLine.Amount = "0";
                        }
                        else
                        {
                            orderLine.Amount = ocr.newAmount;
                        }
                        if (ocr.newReceiptNo.Equals(""))
                        {
                            orderLine.ReceiptNo = "0";
                        }
                        else
                        {
                            orderLine.ReceiptNo = ocr.newReceiptNo;
                        }
                    }
                    orderLine.PictureURL = pictureURL;
                    orderHead.Updated = true;
                    _deliveryContext.SaveChanges();
                } catch
                {
                }
               
            }
        }

    }
}
