using EdcentralizedNet.DataAccess;
using EdcentralizedNet.HttpClients;
using EdcentralizedNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdcentralizedNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EtherscanController : ControllerBase
    {
        private readonly ILogger<EtherscanController> _logger;
        private readonly IEtherscanDA _etherscanDA;

        public EtherscanController(ILogger<EtherscanController> logger, IEtherscanDA etherscanDA)
        {
            _logger = logger;
            _etherscanDA = etherscanDA;
        }

        [HttpGet]
        public async Task<IEnumerable<ERC721Transfer>> Get(string accountAddress)
        {
            //Temp address
            accountAddress = "0xCCEc25758b6db66C4abD31E5333658FcF222dc26";
            return await _etherscanDA.GetERC721TransfersForAccount(accountAddress);
        }
    }
}
