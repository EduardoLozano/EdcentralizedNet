using EdcentralizedNet.EtherscanModels;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public interface IEtherscanManager
    {
        Task<EtherscanTransaction> GetEthTransaction(string transactionHash);
    }
}
