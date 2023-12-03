namespace eShop.Distribution.Services
{
    public class ViberFormatter : ITextFormatter
    {
        public string Strikethrough(string text)
        {
            var result = string.Join(' ', text.Split(' ').Select(unit => $"~{unit}~"));
            return result;
        }

        public string Link(string text, string url)
        {
            return $"{text}\n{url}";
        }
    }
}
