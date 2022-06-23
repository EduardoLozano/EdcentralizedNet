using EdcentralizedNet.Business;
using EdcentralizedNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdcentralizedNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NFTAssetController : ControllerBase
    {
        private readonly ILogger<NFTAssetController> _logger;
        private readonly INFTManager _nftManager;

        public NFTAssetController(ILogger<NFTAssetController> logger, INFTManager nftManager)
        {
            _logger = logger;
            _nftManager = nftManager;
        }

        [HttpGet]
        public async Task<CursorPagedList<NFTAsset>> Get(string accountAddress, string pageCursor)
        {
            //Temp address
            accountAddress = "0xCCEc25758b6db66C4abD31E5333658FcF222dc26";
            var nftAssets = await _nftManager.GetNFTAssetPage(accountAddress, pageCursor);
            return nftAssets;
        }
    }
}
