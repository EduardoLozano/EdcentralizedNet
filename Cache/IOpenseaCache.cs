using EdcentralizedNet.Models;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public interface IOpenseaCache
    {
        Task<OSStats> GetStatsForCollection(string collectionSlug);
        Task SetStatsForCollection(string collectionSlug, OSStats stats);
        Task RemoveStatsForCollection(string collectionSlug);

        Task<OSEvent> GetAssetMintEvent(string contractAddress, string tokenId);
        Task SetAssetMintEvent(string contractAddress, string tokenId, OSEvent aEvent);
        Task RemoveAssetMintEvent(string contractAddress, string tokenId);
    }
}
