using EdcentralizedNet.Models;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public interface IEtherscanCache
    {
        Task<EthTransaction> GetEthTransaction(string transactionHash);
        Task SetEthTransaction(string transactionHash, EthTransaction transaction);
        Task RemoveEthTransaction(string transactionHash);
    }
}
