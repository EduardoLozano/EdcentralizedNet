using EdcentralizedNet.Business;
using EdcentralizedNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EdcentralizedNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return await _nftManager.GetPortfolioInformation(accountAddress);
        }
    }
}
