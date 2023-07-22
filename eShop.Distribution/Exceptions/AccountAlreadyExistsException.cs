namespace eShop.Distribution.Exceptions
{
    public class AccountAlreadyExistsException : Exception
    {
        public AccountAlreadyExistsException() : base("The same account already exists in the system")
        {
        }
    }
}
