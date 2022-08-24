using EdcentralizedNet.Business;
using EdcentralizedNet.Helpers;
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
    [ApiController]
    [Route("api/[controller]")]
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
            //Testing address that has thousands of NFTs
            //accountAddress = "0xeEE5Eb24E7A0EA53B75a1b9aD72e7D20562f4283";
            var nftAssets = await _nftManager.GetNFTAssetPage(accountAddress, pageNumber, pageCursor);
            return nftAssets;
        }
    }
}
