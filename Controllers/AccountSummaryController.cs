using EdcentralizedNet.Business;
using EdcentralizedNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EdcentralizedNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountSummaryController : ControllerBase
    {
        private readonly ILogger<AccountSummaryController> _logger;
        private readonly IAccountSummaryManager _accountSummaryManager;

        public AccountSummaryController(ILogger<AccountSummaryController> logger, IAccountSummaryManager accountSummaryManager)
        {
            _logger = logger;
            _accountSummaryManager = accountSummaryManager;
        }

        [HttpGet]
        public async Task<AccountSummaryVM> Get(string walletAddress, int pageNumber, int pageSize = 20)
        {
            //Testing address that has thousands of NFTs
            //accountAddress = "0xeEE5Eb24E7A0EA53B75a1b9aD72e7D20562f4283";
            var entity = await _accountSummaryManager.GetAccountSummaryForAccount(walletAddress);
            return entity;
        }
    }
}
