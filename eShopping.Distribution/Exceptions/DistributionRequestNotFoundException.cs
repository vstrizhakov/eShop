namespace eShopping.Distribution.Exceptions
{
    public class DistributionRequestNotFoundException : Exception
    {
        public DistributionRequestNotFoundException() : base("Requested distribution request not found")
        {
        }
    }
}
