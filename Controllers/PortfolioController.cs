using EdcentralizedNet.Business;
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
    public class PortfolioController : ControllerBase
    {
        private readonly ILogger<PortfolioController> _logger;
        private readonly INFTManager _nftManager;

        public PortfolioController(ILogger<PortfolioController> logger, INFTManager nftManager)
        {
            _logger = logger;
            _nftManager = nftManager;
        }

        [HttpGet]
        public async Task<PortfolioInformation> Get(string accountAddress)
        {
            //Temp address
            accountAddress = "0xCCEc25758b6db66C4abD31E5333658FcF222dc26";
            var tokens = await _nftManager.GetAllNFTForAccount(accountAddress);
            return new PortfolioInformation(tokens);
        }
    }
}
