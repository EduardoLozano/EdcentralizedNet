using EdcentralizedNet.Models;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public interface IEtherscanCache
    {
        Task<EtherscanTransaction> GetEthTransaction(string transactionHash);
        Task SetEthTransaction(string transactionHash, EtherscanTransaction transaction);
        Task RemoveEthTransaction(string transactionHash);
    }
}
