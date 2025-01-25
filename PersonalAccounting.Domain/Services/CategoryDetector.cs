namespace PersonalAccounting.Domain.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using PersonalAccounting.Domain.Data;

    public class CategoryDetector
    {
        private Dictionary<string, string> categoryKeywords = new();

        public CategoryDetector(List<ReceiptItem> existingItems)
        {
            // Populate the dictionary from existing data
            foreach (var item in existingItems.Where(i => !string.IsNullOrEmpty(i.Category)))
            {
                //Normalize the description
                string normalizedDescription = NormalizeText(item.Description);
                if (!categoryKeywords.ContainsKey(normalizedDescription))
                {
                    categoryKeywords.Add(normalizedDescription, item.Category);
                }
            }
        }

        private string NormalizeText(string text)
        {
            return text.Trim().ToLower();
        }

        public string DetectCategory(string itemDescription)
        {
            string normalizedDescription = NormalizeText(itemDescription);
            // 1. Exact Match
            if (categoryKeywords.ContainsKey(normalizedDescription))
            {
                return categoryKeywords[normalizedDescription];
            }

            // 2. Keyword Matching (Contains)
            foreach (var kvp in categoryKeywords)
            {
                if (normalizedDescription.Contains(kvp.Key))
                {
                    return kvp.Value;
                }
            }

            // 3. Fuzzy Matching (Using FuzzySharp)
            var bestMatch = FuzzySharp.Process.ExtractOne(normalizedDescription, categoryKeywords.Keys);
            if (bestMatch != null && bestMatch.Score >= 80) // Adjust the score threshold as needed
            {
                return categoryKeywords[bestMatch.Value];
            }

            // 4. Default Category (If no match is found)
            return "Other";
        }
    }
}
