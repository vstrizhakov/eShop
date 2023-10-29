using System.Text.RegularExpressions;

namespace eShop.Distribution.Services
{
    public class TelegramFormatter : ITextFormatter
    {
        private static readonly Regex EscapeRegex = new Regex(@"_|\*|\[|\]|\(|\)|~|`|>|#|\+|-|=|\||\{|\}|\.|!");

        public string Link(string text, string url)
        {
            return $"[{Escape(text)}]({url})";
        }

        public string Strikethrough(string text)
        {
            return $"~{Escape(text)}~";
        }

        public string Escape(string text)
        {
            var result = EscapeRegex.Replace(text, @"\$0");
            return result;
        }
    }
}
