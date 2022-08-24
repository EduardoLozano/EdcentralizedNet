using EdcentralizedNet.Business;
using EdcentralizedNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdcentralizedNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountTokenController : ControllerBase
    {
        private readonly ILogger<AccountTokenController> _logger;
        private readonly IAccountTokenManager _accountTokenManager;

        public AccountTokenController(ILogger<AccountTokenController> logger, IAccountTokenManager accountTokenManager)
        {
            _logger = logger;
            _accountTokenManager = accountTokenManager;
        }

        [HttpGet]
        public async Task<IEnumerable<AccountTokenVM>> Get(string walletAddress, int pageNumber, int pageSize = 20)
        {
            //Testing address that has thousands of NFTs
            //accountAddress = "0xeEE5Eb24E7A0EA53B75a1b9aD72e7D20562f4283";
            var entities = await _accountTokenManager.GetAccountTokenPageAsync(walletAddress, pageNumber, pageSize);
            return entities;
        }
    }
}
