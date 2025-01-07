using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.Extensions.Logging;
using PersonalAccounting.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccounting.Domain.Services.OCR
{
    public class FormRecognizerService
    {
        private readonly ILogger<FormRecognizerService> logger;

        public FormRecognizerService(ILogger<FormRecognizerService> logger)
        {
            this.logger=logger;
        }

        public async Task<Receipt> ExtractReceiptInfo(Stream myBlob)
        {
            //log.LogInformation($"Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            // Form Recognizer setup
            string endpoint = Environment.GetEnvironmentVariable($"AZURE_DOCUMENTAI_ENDPOINT{GlobalConfigs.ProdSuffix}")!;
            string apiKey = Environment.GetEnvironmentVariable($"AZURE_DOCUMENTAI_APIKEY{GlobalConfigs.ProdSuffix}")!;
            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

            // Analyze the receipt
            AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-receipt", myBlob); // Use the prebuilt receipt model
            AnalyzeResult result = operation.Value;

            Receipt receipt = new Receipt();

            // Process the extracted data
            foreach (var document in result.Documents)
            {
                if (document.Fields.TryGetValue("MerchantName", out var merchantNameField))
                {
                    receipt.MerchantName = merchantNameField.Value.AsString();
                    //log.LogInformation($"Merchant Name: {merchantName}");
                }

                if (document.Fields.TryGetValue("Items", out var itemsField))
                {
                    try
                    {
                        var lst = itemsField.Value.AsList();
                        foreach (var row in lst)
                        {
                            var fields = row.Value.AsDictionary();
                            ReceiptItem item = new ReceiptItem();

                            if (fields.ContainsKey("Description")) item.Description = fields["Description"].Content;
                            if (fields.ContainsKey("Price")) item.UnitPrice = (decimal)fields["Price"].Value.AsDouble();
                            if (fields.ContainsKey("Quantity")) item.Quantity = (decimal)fields["Quantity"].Value.AsDouble();
                            if (fields.ContainsKey("Quantity")) item.Quantity = (decimal)fields["Quantity"].Value.AsDouble();
                            if (fields.ContainsKey("TotalPrice")) item.TotalPrice = (decimal)fields["TotalPrice"].Value.AsDouble();

                            receipt.Items.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        //logg
                    }
                }
                // ... Extract other fields (Total, Date, Items, etc.)
                if (document.Fields.TryGetValue("Total", out var totalField))
                {
                    receipt.TotalAmount = (decimal)totalField.Value.AsDouble();
                    //log.LogInformation($"Total: {total}");
                }
                if (document.Fields.TryGetValue("TransactionDate", out var transactionDateField))
                {
                    receipt.Date = transactionDateField.Value.AsDate().DateTime;
                    //log.LogInformation($"Transaction Date: {transactionDate}");
                }
                if (document.Fields.TryGetValue("TransactionTime", out var transactionTimeField))
                {
                    receipt.Time = transactionTimeField.Value.AsTime();
                    //log.LogInformation($"Transaction Time: {transactionDate}");
                }
            }

            return receipt;
        }
    }

}
