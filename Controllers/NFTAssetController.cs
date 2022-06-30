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
        public async Task<CursorPagedList<NFTAsset>> Get(string accountAddress, int pageNumber, string pageCursor)
        {
            var nftAssets = await _nftManager.GetNFTAssetPage(accountAddress, pageNumber, pageCursor);
            return nftAssets;
        }
    }
}
