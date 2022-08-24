using EdcentralizedNet.OpenseaModels;
using System;

namespace EdcentralizedNet.Models
{
    public class CollectionStats
    {
        public int CollectionStatsId { get; set; }
        public string CollectionSlug { get; set; }
        public decimal FloorPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public CollectionStats() { }

        public CollectionStats(string collectionSlug, OSStats stats)
        {
            CollectionSlug = collectionSlug;
            
            if(stats != null && stats.floor_price.HasValue)
            {
                FloorPrice = stats.floor_price.Value;
            }
        }
    }
}
