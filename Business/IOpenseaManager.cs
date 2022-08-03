using EdcentralizedNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public interface IOpenseaManager
    {
        Task<OSAssetList> GetAssetsForAccount(string accountAddress, string cursor = null);
    }
}
