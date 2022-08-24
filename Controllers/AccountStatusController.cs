using EdcentralizedNet.Business;
using EdcentralizedNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EdcentralizedNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountStatusController : ControllerBase
    {
        private readonly ILogger<AccountStatusController> _logger;
        private readonly INFTManager _nftManager;

        public AccountStatusController(ILogger<AccountStatusController> logger, INFTManager nftManager)
        {
            _logger = logger;
            _nftManager = nftManager;
        }

        [HttpGet]
        public async Task<AccountStatusVM> Get(string accountAddress)
        {
            //Testing address that has thousands of NFTs
            //accountAddress = "0xeEE5Eb24E7A0EA53B75a1b9aD72e7D20562f4283";
            return await _nftManager.GetAccountStatus(accountAddress);
        }
    }
}
