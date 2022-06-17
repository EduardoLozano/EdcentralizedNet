using EdcentralizedNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdcentralizedNet.DataAccess
{
    public interface IOpenseaDA
    {
        Task<IEnumerable<OSCollection>> GetCollectionsForAccount(string accountAddress);
        Task<OSAssetList> GetAssetsForAccount(string accountAddress, string cursor = null);
    }
}
