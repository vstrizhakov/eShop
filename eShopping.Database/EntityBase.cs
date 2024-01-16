namespace eShopping.Database
{
    public abstract class EntityBase
    {
        private string? _partitionKeyOverride;

        public Guid Id { get; set; } = Guid.NewGuid();
        public string PartitionKey
        {
            get
            {
                var partitionKey = _partitionKeyOverride;
                if (partitionKey == null)
                {
                    partitionKey = GetPartitionKey();
                }

                return partitionKey;
            }
            set
            {
                _partitionKeyOverride = value;
            }
        }


        protected virtual string GetPartitionKey()
        {
            return Id.ToString();
        }

        protected string UseDiscriminator()
        {
            return GetType().Name;
        }

        protected string UseGuid(Guid guid)
        {
            return guid.ToString();
        }
    }
}
