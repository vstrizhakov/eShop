namespace eShopping.Distribution.Entities
{
    public class EmbeddedCurrency : ICloneable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
