using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PowerAppToOCR
{
    public class Formrecognizer
    {
        private static readonly string endpoint = "https://bonocr.cognitiveservices.azure.com/";
        private static readonly string apiKey = "03c61a0e17c844bdbb0749c93550c049";
        private static readonly AzureKeyCredential credential = new AzureKeyCredential(apiKey);
        public string newReceiptNo = "";
        public string newAmount = "";

        public void Run(string pPictureUri)
        {
            var recognizeContent = RecognizeContent(AuthenticateClient(), pPictureUri);
            Task.WaitAll(recognizeContent);

        }

        private FormRecognizerClient AuthenticateClient()
        {
            var credential = new AzureKeyCredential(apiKey);
            var client = new FormRecognizerClient(new Uri(endpoint), credential);
            return client;
        }

        private async Task RecognizeContent(FormRecognizerClient recognizerClient, string pPictureUri)
        {
            string xLine = "";
            FormPageCollection formPages = await recognizerClient
                .StartRecognizeContentFromUri(new Uri(pPictureUri))
                .WaitForCompletionAsync();
            foreach (FormPage page in formPages)
            {
                Console.WriteLine($"Form Page {page.PageNumber} has {page.Lines.Count} lines.");
                for (int i = 0; i < page.Lines.Count; i++)
                {
                    FormLine line = page.Lines[i];
                    Console.WriteLine($"    Line {i} has {line.Words.Count} word{(line.Words.Count > 1 ? "s" : "")}, and text: '{line.Text}'.");
                    if (xLine.Length > 15)
                    {
                        int distanceReceiptNo = CalcLevenshteinDistance(xLine.Substring(0, 15), "lfd. Nr./Zähler");
                        int distanceAmount = CalcLevenshteinDistance(xLine.Substring(0, 5), "Menge");
                        if ((distanceReceiptNo != 0) && (distanceReceiptNo < 6))
                        {
                            newReceiptNo = line.Text;
                        }
                        else if ((distanceAmount != 0) && (distanceAmount < 2))
                        {
                            string[] amounts = line.Text.Split(" ");
                            newAmount = amounts[0];
                        }
                    }
                    else if (xLine.Length >= 5)
                    {
                        int distanceAmount = CalcLevenshteinDistance(xLine, "Menge");
                        if ((distanceAmount != 0) && (distanceAmount < 2))
                        {
                            string[] amounts = line.Text.Split(" ");
                            newAmount = amounts[0];
                        }
                    }

                    if (newAmount == "")
                    {
                        if (xLine.Contains("eng"))
                        {
                            string[] amounts = line.Text.Split(" ");
                            newAmount = amounts[0];
                        }
                    }
                    if (newReceiptNo == "")
                    {
                        if (xLine.Contains("hle"))
                        {
                            newReceiptNo = line.Text;
                        }
                    }
                    xLine = line.Text;
                }
            }
        }

        public int CalcLevenshteinDistance(string a, string b)
        {
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
            {
                return 0;
            }
            if (String.IsNullOrEmpty(a))
            {
                return b.Length;
            }
            if (String.IsNullOrEmpty(b))
            {
                return a.Length;
            }
            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            return distances[lengthA, lengthB];
        }
    }
}
