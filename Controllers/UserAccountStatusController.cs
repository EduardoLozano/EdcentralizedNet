using EdcentralizedNet.Business;
using EdcentralizedNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EdcentralizedNet.Controllers
{
    [Route("api/useraccount/status")]
    [ApiController]
    public class UserAccountStatusController : ControllerBase
    {
        private readonly ILogger<UserAccountStatusController> _logger;
        private readonly INFTManager _nftManager;

        public UserAccountStatusController(ILogger<UserAccountStatusController> logger, INFTManager nftManager)
        {
            _logger = logger;
            _nftManager = nftManager;
        }

        [HttpGet]
        public async Task<AccountStatusResponse> Get(string accountAddress)
        {
            //Testing address that has thousands of NFTs
            //accountAddress = "0xeEE5Eb24E7A0EA53B75a1b9aD72e7D20562f4283";
            return await _nftManager.GetAccountStatus(accountAddress);
        }
    }
}
