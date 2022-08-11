using EdcentralizedNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public interface IOpenseaManager
    {
        Task<OSAssetList> GetAssetPageForAccount(string accountAddress, string cursor = null);
        Task<List<OSAsset>> GetAllAssetsForAccount(string accountAddress);
    }
}
