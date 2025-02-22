﻿using eShopping.Database;

namespace eShopping.Distribution.Entities
{
    public class Distribution : EntityBase
    {
        public Guid AnnouncerId { get; set; }

        public ICollection<DistributionGroup> Targets { get; set; } = new List<DistributionGroup>();

        protected override string GetPartitionKey()
        {
            return UseGuid(AnnouncerId);
        }
    }
}
