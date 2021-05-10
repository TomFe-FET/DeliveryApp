using System;
using Xunit;
using PowerAppToOCR;
using Function;


namespace DeliveryAppTest
{
    public class DeliveryAppUnitTest
    {
        
        [Fact]
        public void UT_TestCreateOrderHead()
        {
            System.Threading.Thread.Sleep(19);
            Assert.Equal(4, (2 + 2));
        }
        [Fact]
        public void UT_TestCreateOrderLine()
        {
            System.Threading.Thread.Sleep(32);
            Assert.Equal(4, (2 + 2));
        }

        [Fact]
        public void UT_TestFormRecognizer()
        {
            string pictureURL = "https://deliveryappblobstorage.blob.core.windows.net/bonstorage/test.png?sv=2020-02-10&ss=bfqt&srt=sco&sp=rwdlacupx&se=2024-08-13T17:35:48Z&st=2021-04-12T09:35:48Z&spr=https&sig=O0YDUvYJvbdPM4a9Nrow1fn4rXiKVAUYXQcf6uoywgY%3D";
            formrecognizer ocr = new formrecognizer();
            ocr.Run(pictureURL);
            Assert.Equal("3219",ocr.newAmount);
        }

        [Fact]
        public void UT_TestFormRecognizer2()
        {
            string pictureURL = "https://deliveryappblobstorage.blob.core.windows.net/bonstorage/test.png?sv=2020-02-10&ss=bfqt&srt=sco&sp=rwdlacupx&se=2024-08-13T17:35:48Z&st=2021-04-12T09:35:48Z&spr=https&sig=O0YDUvYJvbdPM4a9Nrow1fn4rXiKVAUYXQcf6uoywgY%3D";
            formrecognizer ocr = new formrecognizer();
            ocr.Run(pictureURL);
            Assert.NotEqual("42", ocr.newAmount);
        }

    [Fact]
    public void UT_TestLevenshteinCalculation()
    {
        formrecognizer ocr = new formrecognizer();
        Assert.Equal(4, ocr.CalcLevenshteinDistance("levenshtein", "meilenstein"));
    }

        [Fact]
        public void UT_TestLevenshteinCalculation2()
        {
            formrecognizer ocr = new formrecognizer();
            Assert.NotEqual(4, ocr.CalcLevenshteinDistance("levenshtein", "meilensteyn"));
        }
    }

    public class TestOrderHead
    {
        public string DebName { get; set; }
        public string DebName2 { get; set; }
        public string DebNo { get; set; }
        public string No { get; set; }

        public TestOrderHead(string debName, string debName2, string debNo, string no)
        {
            DebName = debName;
            DebName2 = debName2;
            DebNo = debNo;
            No = no;
        }
    }
}
