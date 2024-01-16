namespace eShopping.Distribution.Services
{
    public interface ITextFormatter
    {
        string Link(string text, string url);
        string Strikethrough(string text);
    }
}