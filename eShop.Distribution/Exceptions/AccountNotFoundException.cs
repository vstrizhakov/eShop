namespace eShop.Distribution.Exceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException() : base("Requested account not found")
        {
        }
    }
}
