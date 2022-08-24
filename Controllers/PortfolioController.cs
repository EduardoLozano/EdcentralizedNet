using EdcentralizedNet.Business;
using EdcentralizedNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EdcentralizedNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<AccountSummary> Get(string accountAddress)
        {
            //Testing address that has thousands of NFTs
            //accountAddress = "0xeEE5Eb24E7A0EA53B75a1b9aD72e7D20562f4283";
            return await _nftManager.GetPortfolioInformation(accountAddress);
        }
    }
}
